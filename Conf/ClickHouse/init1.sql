-- CREATE DATABASE IF NOT EXISTS default;
-- 
-- CREATE TABLE IF NOT EXISTS default.otel_logs (
--                                                  timestamp DateTime DEFAULT now(),
--     trace_id String,
--     span_id String,
--     severity String,
--     body String
--     ) ENGINE = MergeTree()
--     ORDER BY timestamp;

-- CREATE DATABASE IF NOT EXISTS default;
-- 
-- CREATE TABLE IF NOT EXISTS default.otel_logs (
--                                                  timestamp DateTime DEFAULT now(),
--     trace_id String,
--     span_id String,
--     severity String,
--     body String
--     ) ENGINE = MergeTree()
--     ORDER BY timestamp;

-- CREATE TABLE default.otel_logs (
--                                    timestamp DateTime DEFAULT now(),
--                                    trace_id String,
--                                    span_id String,
--                                    severity String,
--                                    service_name String,
--                                    body String
-- ) ENGINE = MergeTree()
-- ORDER BY timestamp;

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