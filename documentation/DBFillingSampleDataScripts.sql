BEGIN;

-- =========================================================================
-- Вставка данных в таблицу Roles
-- =========================================================================
INSERT INTO "Roles" ("id", "name", "is_deleted") VALUES
(gen_random_uuid(), 'Admin', FALSE),
(gen_random_uuid(), 'User', FALSE);

-- =========================================================================
-- Вставка данных в таблицу Users
-- =========================================================================
INSERT INTO "Users" ("id", "first_name", "last_name", "email", "phone_number", "password_hash", "is_deleted", "created_at", "updated_at") VALUES
(gen_random_uuid(), 'John', 'Doe', 'john.doe@example.com', '1234567890', 'john.doe@example.com', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), 'Jane', 'Smith', 'jane.smith@example.com', '9876543210', 'jane.smith@example.com', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), 'Alice', 'Johnson', 'alice.johnson@example.com', '1231231234', 'alice.johnson@example.com', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), 'Bob', 'Brown', 'bob.brown@example.com', '3213213214', 'bob.brown@example.com', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), 'Eve', 'Davis', 'eve.davis@example.com', '5555555555', 'eve.davis@example.com', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- =========================================================================
-- Вставка данных в таблицу UserRoles
-- =========================================================================
INSERT INTO "UserRoles" ("user_id", "role_id", "is_deleted") VALUES
((SELECT "id" FROM "Users" WHERE "email" = 'john.doe@example.com'), (SELECT "id" FROM "Roles" WHERE "name" = 'Admin'), FALSE),
((SELECT "id" FROM "Users" WHERE "email" = 'jane.smith@example.com'), (SELECT "id" FROM "Roles" WHERE "name" = 'User'), FALSE),
((SELECT "id" FROM "Users" WHERE "email" = 'alice.johnson@example.com'), (SELECT "id" FROM "Roles" WHERE "name" = 'User'), FALSE),
((SELECT "id" FROM "Users" WHERE "email" = 'bob.brown@example.com'), (SELECT "id" FROM "Roles" WHERE "name" = 'User'), FALSE),
((SELECT "id" FROM "Users" WHERE "email" = 'eve.davis@example.com'), (SELECT "id" FROM "Roles" WHERE "name" = 'User'), FALSE);

-- =========================================================================
-- Вставка данных в таблицу CarBrands
-- =========================================================================
INSERT INTO "CarBrands" ("id", "name", "country", "is_deleted") VALUES
(gen_random_uuid(), 'Toyota', 'Japan', FALSE),
(gen_random_uuid(), 'Ford', 'USA', FALSE),
(gen_random_uuid(), 'BMW', 'Germany', FALSE),
(gen_random_uuid(), 'Hyundai', 'South Korea', FALSE),
(gen_random_uuid(), 'Fiat', 'Italy', FALSE);

-- =========================================================================
-- Вставка данных в таблицу CarModels
-- =========================================================================
INSERT INTO "CarModels" ("id", "name", "is_deleted") VALUES
(gen_random_uuid(), 'Corolla', FALSE),
(gen_random_uuid(), 'Focus', FALSE),
(gen_random_uuid(), '3 Series', FALSE),
(gen_random_uuid(), 'Elantra', FALSE),
(gen_random_uuid(), 'Panda', FALSE);

-- =========================================================================
-- Вставка данных в таблицу CarBodyTypes
-- =========================================================================
INSERT INTO "CarBodyTypes" ("id", "name", "is_deleted") VALUES
(gen_random_uuid(), 'Sedan', FALSE),
(gen_random_uuid(), 'SUV', FALSE),
(gen_random_uuid(), 'Hatchback', FALSE),
(gen_random_uuid(), 'Coupe', FALSE),
(gen_random_uuid(), 'Convertible', FALSE);

-- =========================================================================
-- Вставка данных в таблицу TransmissionTypes
-- =========================================================================
INSERT INTO "TransmissionTypes" ("id", "name", "is_deleted") VALUES
(gen_random_uuid(), 'Automatic', FALSE),
(gen_random_uuid(), 'Manual', FALSE);

-- =========================================================================
-- Вставка данных в таблицу DriveTypes
-- =========================================================================
INSERT INTO "DriveTypes" ("id", "name", "is_deleted") VALUES
(gen_random_uuid(), 'FWD', FALSE),
(gen_random_uuid(), 'RWD', FALSE),
(gen_random_uuid(), 'AWD', FALSE);

-- =========================================================================
-- Вставка данных в таблицу Cars
-- =========================================================================
INSERT INTO "Cars" ("id", "brand_id", "model_id", "body_type_id", "transmission_type_id", "drive_type_id", "seats", "mileage", "is_deleted", "created_at", "updated_at") VALUES
(gen_random_uuid(), (SELECT "id" FROM "CarBrands" WHERE "name" = 'Toyota'), (SELECT "id" FROM "CarModels" WHERE "name" = 'Corolla'), (SELECT "id" FROM "CarBodyTypes" WHERE "name" = 'Sedan'), (SELECT "id" FROM "TransmissionTypes" WHERE "name" = 'Automatic'), (SELECT "id" FROM "DriveTypes" WHERE "name" = 'FWD'), 5, 10000, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), (SELECT "id" FROM "CarBrands" WHERE "name" = 'Ford'), (SELECT "id" FROM "CarModels" WHERE "name" = 'Focus'), (SELECT "id" FROM "CarBodyTypes" WHERE "name" = 'Hatchback'), (SELECT "id" FROM "TransmissionTypes" WHERE "name" = 'Manual'), (SELECT "id" FROM "DriveTypes" WHERE "name" = 'RWD'), 5, 20000, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), (SELECT "id" FROM "CarBrands" WHERE "name" = 'BMW'), (SELECT "id" FROM "CarModels" WHERE "name" = '3 Series'), (SELECT "id" FROM "CarBodyTypes" WHERE "name" = 'Sedan'), (SELECT "id" FROM "TransmissionTypes" WHERE "name" = 'Automatic'), (SELECT "id" FROM "DriveTypes" WHERE "name" = 'AWD'), 5, 15000, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), (SELECT "id" FROM "CarBrands" WHERE "name" = 'Hyundai'), (SELECT "id" FROM "CarModels" WHERE "name" = 'Elantra'), (SELECT "id" FROM "CarBodyTypes" WHERE "name" = 'SUV'), (SELECT "id" FROM "TransmissionTypes" WHERE "name" = 'Manual'), (SELECT "id" FROM "DriveTypes" WHERE "name" = 'FWD'), 7, 12000, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(gen_random_uuid(), (SELECT "id" FROM "CarBrands" WHERE "name" = 'Fiat'), (SELECT "id" FROM "CarModels" WHERE "name" = 'Panda'), (SELECT "id" FROM "CarBodyTypes" WHERE "name" = 'Hatchback'), (SELECT "id" FROM "TransmissionTypes" WHERE "name" = 'Manual'), (SELECT "id" FROM "DriveTypes" WHERE "name" = 'RWD'), 4, 8000, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- =========================================================================
-- Вставка данных в таблицу Features
-- =========================================================================
INSERT INTO "Features" ("id", "name", "is_deleted") VALUES
(gen_random_uuid(), 'Air Conditioning', FALSE),
(gen_random_uuid(), 'Heated Seats', FALSE),
(gen_random_uuid(), 'Navigation System', FALSE),
(gen_random_uuid(), 'Backup Camera', FALSE),
(gen_random_uuid(), 'Bluetooth', FALSE);

-- =========================================================================
-- Вставка данных в таблицу CarFeatures
-- =========================================================================
INSERT INTO "CarFeatures" ("car_id", "feature_id", "is_deleted") VALUES
((SELECT "id" FROM "Cars" WHERE "mileage" = 10000), (SELECT "id" FROM "Features" WHERE "name" = 'Air Conditioning'), FALSE),
((SELECT "id" FROM "Cars" WHERE "mileage" = 20000), (SELECT "id" FROM "Features" WHERE "name" = 'Heated Seats'), FALSE),
((SELECT "id" FROM "Cars" WHERE "mileage" = 15000), (SELECT "id" FROM "Features" WHERE "name" = 'Navigation System'), FALSE),
((SELECT "id" FROM "Cars" WHERE "mileage" = 12000), (SELECT "id" FROM "Features" WHERE "name" = 'Backup Camera'), FALSE),
((SELECT "id" FROM "Cars" WHERE "mileage" = 8000), (SELECT "id" FROM "Features" WHERE "name" = 'Bluetooth'), FALSE);

-- =========================================================================
-- Вставка данных в таблицу RentOffers
-- =========================================================================
INSERT INTO "RentOffers" ("id", "user_id", "car_id", "price_per_day", "location", "created_at", "updated_at", "description", "is_available", "is_deleted") VALUES
(gen_random_uuid(), (SELECT "id" FROM "Users" WHERE "email" = 'john.doe@example.com'), (SELECT "id" FROM "Cars" WHERE "mileage" = 10000), 50.00, 'New York, NY', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Clean and reliable.', TRUE, FALSE),
(gen_random_uuid(), (SELECT "id" FROM "Users" WHERE "email" = 'jane.smith@example.com'), (SELECT "id" FROM "Cars" WHERE "mileage" = 20000), 40.00, 'Los Angeles, CA', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Economical and comfortable.', TRUE, FALSE),
(gen_random_uuid(), (SELECT "id" FROM "Users" WHERE "email" = 'alice.johnson@example.com'), (SELECT "id" FROM "Cars" WHERE "mileage" = 15000), 70.00, 'Chicago, IL', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Luxury and performance.', TRUE, FALSE),
(gen_random_uuid(), (SELECT "id" FROM "Users" WHERE "email" = 'bob.brown@example.com'), (SELECT "id" FROM "Cars" WHERE "mileage" = 12000), 60.00, 'Houston, TX', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Spacious and practical.', TRUE, FALSE),
(gen_random_uuid(), (SELECT "id" FROM "Users" WHERE "email" = 'eve.davis@example.com'), (SELECT "id" FROM "Cars" WHERE "mileage" = 8000), 30.00, 'Miami, FL', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Compact and efficient.', TRUE, FALSE);

COMMIT;