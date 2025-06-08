CREATE TABLE `stamp_palettes` (
    `id`            CHAR(36)    NOT NULL    PRIMARY KEY,
    `user_id`       CHAR(36)    NOT NULL,
    `is_public`     BOOLEAN     NOT NULL    DEFAULT 0,
    `created_at`    DATETIME    NOT NULL    DEFAULT CURRENT_TIMESTAMP,
    `updated_at`    DATETIME    NOT NULL    DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP
)   DEFAULT CHARSET=utf8mb4;
ALTER TABLE `stamp_palettes` ADD INDEX (`user_id`);
ALTER TABLE `stamp_palettes` ADD INDEX (`is_public`);
ALTER TABLE `stamp_palettes` ADD INDEX (`user_id`, `is_public`);

CREATE TABLE `stamp_palette_subscriptions` (
    `id`                CHAR(36)    NOT NULL    PRIMARY KEY,
    `user_id`           CHAR(36)    NOT NULL,
    `palette_id`        CHAR(36)    NOT NULL,
    `copied_palette_id` CHAR(36)    NOT NULL,
    `synced_at`         DATETIME    NOT NULL    DEFAULT CURRENT_TIMESTAMP,
    `created_at`        DATETIME    NOT NULL    DEFAULT CURRENT_TIMESTAMP,
    `updated_at`        DATETIME    NOT NULL    DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE (`user_id`, `palette_id`)
)   DEFAULT CHARSET=utf8mb4;
ALTER TABLE `stamp_palette_subscriptions` ADD INDEX (`user_id`);
ALTER TABLE `stamp_palette_subscriptions` ADD INDEX (`palette_id`);
ALTER TABLE `stamp_palette_subscriptions` ADD INDEX (`copied_palette_id`);
