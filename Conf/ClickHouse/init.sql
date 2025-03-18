DROP TABLE IF EXISTS default.otel_logs;
CREATE TABLE default.otel_logs (
                                   timestamp DateTime DEFAULT now(),
                                   appname String,
                                   facility String,
                                   hostname String,
                                   message String,
                                   msgid String,
                                   procid Int32,
                                   severity String,
                                   version Int32
) ENGINE = MergeTree()
ORDER BY timestamp;

-- Traces table
CREATE TABLE IF NOT EXISTS traces (
                                      traceId String,
                                      spanId String,
                                      traceFlags UInt32,
                                      spanName String,
                                      parentSpanId String,
                                      startTime DateTime64(9, 'UTC'),
    endTime DateTime64(9, 'UTC'),
    duration UInt64,
    attributes Map(String, String),
    resourceAttributes Map(String, String),
    events Array(Tuple(DateTime64(9, 'UTC'), String, Map(String, String))),
    links Array(Tuple(String, String, Map(String, String)))
    ) ENGINE = MergeTree()
    PARTITION BY toDate(startTime)
    ORDER BY (traceId, startTime);

-- Metrics table
CREATE TABLE IF NOT EXISTS metrics (
                                       metricName String,
                                       timestamp DateTime64(9, 'UTC'),
    attributes Map(String, String),
    resourceAttributes Map(String, String),
    value Float64,
    unit String,
    exemplars Array(Tuple(String, String, Float64, DateTime64(9, 'UTC')))
    ) ENGINE = MergeTree()
    PARTITION BY toDate(timestamp)
    ORDER BY (metricName, timestamp);