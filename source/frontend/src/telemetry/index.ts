import {
  Attributes,
  Context,
  context,
  metrics,
  Span,
  SpanKind,
  SpanStatusCode,
  trace,
  Tracer,
} from '@opentelemetry/api';
import { W3CTraceContextPropagator } from '@opentelemetry/core';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { browserDetector } from '@opentelemetry/opentelemetry-browser-detector';
import { detectResourcesSync, Resource } from '@opentelemetry/resources';
import { MeterProvider, PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import {
  BatchSpanProcessor,
  ConsoleSpanExporter,
  SimpleSpanProcessor,
  SpanProcessor,
  WebTracerProvider,
} from '@opentelemetry/sdk-trace-web';
import {
  ATTR_EXCEPTION_MESSAGE,
  ATTR_EXCEPTION_STACKTRACE,
  ATTR_EXCEPTION_TYPE,
  ATTR_SERVICE_NAME,
  ATTR_SERVICE_VERSION,
  ATTR_USER_AGENT_ORIGINAL,
} from '@opentelemetry/semantic-conventions';
import { ATTR_SERVICE_INSTANCE_ID } from '@opentelemetry/semantic-conventions/incubating';
import { v4 as uuidv4 } from 'uuid';

import { DisableTelemetry } from '@/utils/config';
import { exists } from '@/utils/utils';

import { TelemetrySettings } from './config';
import { BrowserAttributesSpanProcessor } from './traces/BrowserAttributesSpanProcessor';
import { UserInfoSpanProcessor } from './traces/UserInfoSpanProcessor';
import { UserAPI } from './users/UserAPI';

// Global provider to capture logged-in user in traces
export const UserTelemetry = UserAPI.getInstance();

export interface SpanOptions {
  attributes?: Attributes;
  kind?: SpanKind;
  /** Parent context to link the span to */
  parentContext?: Context;
}

/**
 * Static facade around OpenTelemetry for browser environments.
 *
 * Usage:
 *   Telemetry.init({ serviceName: "my-app", serviceVersion: "1.0.0" });
 *
 *   // Traces
 *   const span = Telemetry.startSpan("user.login", { attributes: { "user.id": "42" } });
 *   try { ... } finally { Telemetry.endSpan(span); }
 *
 *   // Or wrap a function
 *   const result = await Telemetry.withSpan("fetchUser", async (span) => {
 *     span.setAttribute("user.id", userId);
 *     return fetchUser(userId);
 *   });
 *
 *   // Metrics
 *   Telemetry.counter("button.click").add(1, { "button.name": "signup" });
 *   Telemetry.histogram("api.latency").record(responseTimeMs, { route: "/api/users" });
 *   Telemetry.gauge("cart.size").record(cartItems.length);
 *
 *   // Cleanup on app teardown
 *   await Telemetry.shutdown();
 */
export class Telemetry {
  private static _initialized = false;
  private static _tracerName = 'browser-tracer';
  private static _meterName = 'browser-meter';
  private static _tracerProvider: WebTracerProvider | null = null;
  private static _meterProvider: MeterProvider | null = null;

  static init(config: TelemetrySettings): void {
    // Wrap the entire initialization in a try/catch to prevent telemetry errors from impacting the app.
    try {
      if (DisableTelemetry) {
        console.info(
          '[Telemetry] Initialization skipped - telemetry is disabled via configuration.',
        );
        return;
      }

      if (Telemetry._initialized) {
        console.warn('[Telemetry] Already initialized. Ignoring duplicate init call.');
        return;
      }

      const {
        appName,
        appVersion = '0.0.0',
        collectorUrl = 'http://localhost:4318',
        metricExportIntervalMs = 30_000,
        traceExportIntervalMs = 5_000,
        resourceAttributes = {},
        debug = false,
      } = config;

      // This is common metadata sent with every metric measurement and trace span, describing the application and environment.
      let resource = new Resource({
        [ATTR_SERVICE_NAME]: appName,
        [ATTR_SERVICE_VERSION]: appVersion,
        [ATTR_SERVICE_INSTANCE_ID]: uuidv4(),
        [ATTR_USER_AGENT_ORIGINAL]: typeof navigator !== 'undefined' ? navigator.userAgent : '',
        'screen.width': window.screen.width,
        'screen.height': window.screen.height,
        ...resourceAttributes,
      });

      const detectedResources = detectResourcesSync({ detectors: [browserDetector] });
      resource = resource.merge(detectedResources);

      // Setup telemetry tracing
      const traceExporter = new OTLPTraceExporter({
        url: new URL('v1/traces', collectorUrl).href,
      });

      const processors: SpanProcessor[] = [];

      // Print telemetry data to the console in debug mode, in addition to sending to the collector.
      if (debug) {
        processors.push(new SimpleSpanProcessor(new ConsoleSpanExporter()));
      }

      // Traces are exported every 5s by default, while batching requests together to reduce network overhead.
      processors.push(
        new BatchSpanProcessor(traceExporter, { scheduledDelayMillis: traceExportIntervalMs }),
        new BrowserAttributesSpanProcessor(),
        new UserInfoSpanProcessor(),
      );

      const tracerProvider = new WebTracerProvider({
        resource,
        spanProcessors: [...processors],
      });

      tracerProvider.register({
        propagator: new W3CTraceContextPropagator(),
      });

      trace.setGlobalTracerProvider(tracerProvider);
      Telemetry._tracerProvider = tracerProvider;

      // Setup telemetry metrics
      const metricExporter = new OTLPMetricExporter({
        url: new URL('/v1/metrics', collectorUrl).href,
      });

      // Metrics are exported every 30s by default.
      const meterProvider = new MeterProvider({
        resource,
        readers: [
          new PeriodicExportingMetricReader({
            exporter: metricExporter,
            exportIntervalMillis: metricExportIntervalMs,
          }),
        ],
      });

      metrics.setGlobalMeterProvider(meterProvider);
      Telemetry._meterProvider = meterProvider;

      Telemetry._initialized = true;
      console.info(
        `[Telemetry] Initialized — service: ${appName} v${appVersion}, collector: ${collectorUrl}`,
      );
    } catch (error) {
      console.error(`[Telemetry] Initialization error: ${error}`);
    }
  }

  /** Flush and shut down both providers. Call on app teardown. */
  static async shutdown(): Promise<void> {
    await Promise.all([
      Telemetry._tracerProvider?.shutdown(),
      Telemetry._meterProvider?.shutdown(),
    ]);
    Telemetry._initialized = false;
  }

  /** Get the underlying OpenTelemetry tracer. */
  static get tracer(): Tracer {
    return trace.getTracer(Telemetry._tracerName);
  }

  /**
   * Start a new span. You are responsible for ending it via `endSpan()`.
   * Prefer `withSpan()` for automatic lifecycle management.
   */
  static startSpan(name: string, options: SpanOptions = {}): Span {
    const { attributes = {}, kind = SpanKind.CLIENT, parentContext } = options;
    const ctx = parentContext ?? context.active();
    return Telemetry.tracer.startSpan(name, { kind, attributes }, ctx);
  }

  /** End a span, optionally recording an error. */
  static endSpan(span: Span, error?: unknown): void {
    if (exists(error)) {
      span.recordException(error instanceof Error ? error : new Error(String(error)));
      span.setStatus({ code: SpanStatusCode.ERROR });
    } else {
      span.setStatus({ code: SpanStatusCode.OK });
    }
    span.end();
  }

  /**
   * Wrap an async (or sync) function in a span.
   * The span is automatically ended — with error recording — when the function settles.
   *
   * @example
   * const data = await Telemetry.withSpan("loadDashboard", async (span) => {
   *   span.setAttribute("user.id", userId);
   *   return fetchDashboard(userId);
   * });
   */
  static async withSpan<T>(
    name: string,
    fn: (span: Span) => T | Promise<T>,
    options: SpanOptions = {},
  ): Promise<T> {
    const span = Telemetry.startSpan(name, options);
    try {
      const result = await fn(span);
      Telemetry.endSpan(span);
      return result;
    } catch (err) {
      Telemetry.endSpan(span, err);
      throw err;
    }
  }

  /**
   * Record a one-off event as a zero-duration span.
   * Useful for marking significant moments (e.g. "user.logout", "feature.enabled").
   */
  static recordEvent(name: string, attributes: Attributes = {}): void {
    const span = Telemetry.startSpan(name, { attributes });
    Telemetry.endSpan(span);
  }

  /**
   * Record an error as a dedicated ERROR span and increment the `error.count` counter.
   *
   * The span is named `"error"` by default but can be overridden via `spanName`.
   *
   * Any extra `attributes` you supply are merged in and also forwarded to the
   * `error.count` counter, making it easy to slice error metrics by dimension
   * (e.g. `{ "http.route": "/api/checkout", "error.handled": "false" }`).
   *
   * @example — basic
   * try {
   *   await placeOrder(cart);
   * } catch (err) {
   *   Telemetry.recordException(err);
   *   throw err;
   * }
   *
   * @example — with extra context
   * Telemetry.recordException(err, {
   *   "http.route": "/api/checkout",
   *   "user.id": userId,
   *   "error.handled": "false",
   * });
   *
   * @example — custom span name
   * Telemetry.recordException(err, { "cart.id": cartId }, "checkout.error");
   */
  static recordException(error: unknown, attributes: Attributes = {}, spanName = 'error'): void {
    const err = error instanceof Error ? error : new Error(String(error));

    // OpenTelemetry semantic convention exception attributes
    const errorAttributes: Attributes = {
      [ATTR_EXCEPTION_TYPE]: err.name,
      [ATTR_EXCEPTION_MESSAGE]: err.message,
      [ATTR_EXCEPTION_STACKTRACE]: err.stack ?? '',
      // Convenience duplicates for quick filtering
      'error.name': err.name,
      ...attributes,
    };

    // Emit a dedicated ERROR span so the error is visible in any trace backend
    const span = Telemetry.startSpan(spanName, { attributes: errorAttributes });
    span.recordException(err);
    span.setStatus({ code: SpanStatusCode.ERROR, message: err.message });
    span.end();

    // Increment the error counter so dashboards can track error rates
    Telemetry.counter('error.count', 'Total number of recorded errors').add(1, {
      [ATTR_EXCEPTION_TYPE]: err.name,
      ...attributes,
    });
  }

  // ── Metrics ───────────────────────────────────────────────────────────────

  /** Get the underlying OpenTelemetry meter. */
  static get meter() {
    return metrics.getMeter(Telemetry._meterName);
  }

  /**
   * Monotonically increasing counter (e.g. page views, button clicks, errors).
   *
   * @example
   * Telemetry.counter("page.view").add(1, { page: "/home" });
   */
  static counter(name: string, description?: string) {
    return Telemetry.meter.createCounter(name, { description });
  }

  /**
   * Up-down counter for values that can increase or decrease (e.g. active connections, queue depth).
   *
   * @example
   * Telemetry.upDownCounter("active.users").add(1);
   * Telemetry.upDownCounter("active.users").add(-1);
   */
  static upDownCounter(name: string, description?: string) {
    return Telemetry.meter.createUpDownCounter(name, { description });
  }

  /**
   * Histogram for recording distributions of values over time (e.g. latency, response size).
   *
   * @example
   * Telemetry.histogram("api.duration_ms").record(elapsed, { route: "/search" });
   */
  static histogram(name: string, description?: string) {
    return Telemetry.meter.createHistogram(name, { description });
  }

  /**
   * Synchronous gauge — record an instantaneous value on demand.
   * Use this when you want to report a known value at a specific moment in code,
   * rather than polling on a schedule. Ideal for request-scoped snapshots
   * (e.g. queue depth at the time of a request, current score, form progress).
   *
   * @example
   * Telemetry.gauge("cart.size").record(cart.length, { "user.id": userId });
   * Telemetry.gauge("upload.progress_pct").record(pct, { "file.name": fileName });
   */
  static gauge(name: string, description?: string) {
    return Telemetry.meter.createGauge(name, { description });
  }

  // ── Convenience helpers ───────────────────────────────────────────────────

  /**
   * Measure how long an async function takes and record it as a histogram.
   *
   * @example
   * const user = await Telemetry.measure("db.query", () => db.getUser(id), { "db.table": "users" });
   */
  static async measure<T>(
    metricName: string,
    fn: () => T | Promise<T>,
    attributes: Attributes = {},
  ): Promise<T> {
    const hist = Telemetry.histogram(`${metricName}.duration_ms`);
    const start = performance.now();
    try {
      return await fn();
    } finally {
      hist.record(performance.now() - start, attributes);
    }
  }

  /**
   * Combine tracing + timing in one call.
   * Creates a span AND records a histogram for the duration.
   *
   * @example
   * const result = await Telemetry.trace("fetchProducts", fetchProducts, { "category": "shoes" });
   */
  static async trace<T>(
    name: string,
    fn: (span: Span) => T | Promise<T>,
    attributes: Attributes = {},
  ): Promise<T> {
    return Telemetry.withSpan(
      name,
      async span => {
        const hist = Telemetry.histogram(`${name}.duration_ms`);
        const start = performance.now();
        try {
          return await fn(span);
        } finally {
          hist.record(performance.now() - start, attributes);
        }
      },
      { attributes },
    );
  }
}
