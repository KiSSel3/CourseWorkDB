BEGIN;

CREATE TABLE "Roles"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(50) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "Roles" ADD PRIMARY KEY("id");
ALTER TABLE "Roles" ADD CONSTRAINT "roles_name_unique" UNIQUE("name");

CREATE TABLE "Users"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "first_name" VARCHAR(50) NOT NULL,
    "last_name" VARCHAR(50) NOT NULL,
    "email" VARCHAR(100) NOT NULL,
    "phone_number" VARCHAR(20) NULL,
    "password_hash" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "Users" ADD PRIMARY KEY("id");
ALTER TABLE "Users" ADD CONSTRAINT "users_email_unique" UNIQUE("email");

CREATE TABLE "UserRoles"(
    "user_id" UUID NOT NULL,
    "role_id" UUID NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("user_id", "role_id")
);
ALTER TABLE "UserRoles" ADD CONSTRAINT "fk_user_roles_user" FOREIGN KEY("user_id") REFERENCES "Users"("id") ON UPDATE CASCADE ON DELETE CASCADE;
ALTER TABLE "UserRoles" ADD CONSTRAINT "fk_user_roles_role" FOREIGN KEY("role_id") REFERENCES "Roles"("id") ON UPDATE CASCADE ON DELETE CASCADE;

CREATE TABLE "CarBrands"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(100) NOT NULL,
    "country" VARCHAR(100) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "CarBrands" ADD PRIMARY KEY("id");
ALTER TABLE "CarBrands" ADD CONSTRAINT "carbrands_name_unique" UNIQUE("name");

CREATE TABLE "CarModels"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(100) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "CarModels" ADD PRIMARY KEY("id");
ALTER TABLE "CarModels" ADD CONSTRAINT "carmodels_name_unique" UNIQUE("name");

CREATE TABLE "CarBodyTypes"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(50) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "CarBodyTypes" ADD PRIMARY KEY("id");
ALTER TABLE "CarBodyTypes" ADD CONSTRAINT "carbodytypes_name_unique" UNIQUE("name");

CREATE TABLE "TransmissionTypes"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(50) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "TransmissionTypes" ADD PRIMARY KEY("id");
ALTER TABLE "TransmissionTypes" ADD CONSTRAINT "transmissiontypes_name_unique" UNIQUE("name");

CREATE TABLE "DriveTypes"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(50) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "DriveTypes" ADD PRIMARY KEY("id");
ALTER TABLE "DriveTypes" ADD CONSTRAINT "drivetypes_name_unique" UNIQUE("name");

CREATE TABLE "Cars"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "brand_id" UUID NOT NULL,
    "model_id" UUID NOT NULL,
    "body_type_id" UUID NOT NULL,
    "transmission_type_id" UUID NOT NULL,
    "drive_type_id" UUID NOT NULL,
    "seats" INTEGER NOT NULL,
    "mileage" INTEGER NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("id")
);
ALTER TABLE "Cars" ADD CONSTRAINT "fk_cars_brand" FOREIGN KEY("brand_id") REFERENCES "CarBrands"("id") ON UPDATE CASCADE ON DELETE RESTRICT;
ALTER TABLE "Cars" ADD CONSTRAINT "fk_cars_model" FOREIGN KEY("model_id") REFERENCES "CarModels"("id") ON UPDATE CASCADE ON DELETE RESTRICT;
ALTER TABLE "Cars" ADD CONSTRAINT "fk_cars_body_type" FOREIGN KEY("body_type_id") REFERENCES "CarBodyTypes"("id") ON UPDATE CASCADE ON DELETE RESTRICT;
ALTER TABLE "Cars" ADD CONSTRAINT "fk_cars_transmission" FOREIGN KEY("transmission_type_id") REFERENCES "TransmissionTypes"("id") ON UPDATE CASCADE ON DELETE RESTRICT;
ALTER TABLE "Cars" ADD CONSTRAINT "fk_cars_drive_type" FOREIGN KEY("drive_type_id") REFERENCES "DriveTypes"("id") ON UPDATE CASCADE ON DELETE RESTRICT;

CREATE TABLE "Features"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "name" VARCHAR(100) NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE "Features" ADD PRIMARY KEY("id");
ALTER TABLE "Features" ADD CONSTRAINT "features_name_unique" UNIQUE("name");

CREATE TABLE "CarFeatures"(
    "car_id" UUID NOT NULL,
    "feature_id" UUID NOT NULL,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("car_id", "feature_id")
);
ALTER TABLE "CarFeatures" ADD CONSTRAINT "fk_car_features_car" FOREIGN KEY("car_id") REFERENCES "Cars"("id") ON UPDATE CASCADE ON DELETE CASCADE;
ALTER TABLE "CarFeatures" ADD CONSTRAINT "fk_car_features_feature" FOREIGN KEY("feature_id") REFERENCES "Features"("id") ON UPDATE CASCADE ON DELETE CASCADE;

CREATE TABLE "RentOffers"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "user_id" UUID NOT NULL,
    "car_id" UUID NOT NULL,
    "price_per_day" DECIMAL(10, 2) NOT NULL,
    "location" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "description" TEXT NULL,
    "is_available" BOOLEAN NOT NULL DEFAULT FALSE,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("id")
);
ALTER TABLE "RentOffers" ADD CONSTRAINT "fk_rent_offers_user" FOREIGN KEY("user_id") REFERENCES "Users"("id") ON UPDATE CASCADE ON DELETE RESTRICT;
ALTER TABLE "RentOffers" ADD CONSTRAINT "fk_rent_offers_car" FOREIGN KEY("car_id") REFERENCES "Cars"("id") ON UPDATE CASCADE ON DELETE RESTRICT;

CREATE TABLE "RentOfferImages"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "rent_offer_id" UUID NOT NULL,
    "image_url" TEXT NOT NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("id")
);
ALTER TABLE "RentOfferImages" ADD CONSTRAINT "fk_rent_offer_images_rent_offer" FOREIGN KEY("rent_offer_id") REFERENCES "RentOffers"("id") ON UPDATE CASCADE ON DELETE CASCADE;

CREATE TYPE "BookingStatus" AS ENUM ('PENDING', 'CONFIRMED', 'CANCELLED', 'COMPLETED');

CREATE TABLE "Bookings"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "user_id" UUID NOT NULL,
    "rent_offer_id" UUID NOT NULL,
    "start_date" DATE NOT NULL,
    "end_date" DATE NOT NULL,
    "total_price" DECIMAL(10, 2) NOT NULL,
    "status" "BookingStatus" NOT NULL DEFAULT 'PENDING',
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("id")
);
ALTER TABLE "Bookings" ADD CONSTRAINT "fk_bookings_user" FOREIGN KEY("user_id") REFERENCES "Users"("id") ON UPDATE CASCADE ON DELETE RESTRICT;
ALTER TABLE "Bookings" ADD CONSTRAINT "fk_bookings_rent_offer" FOREIGN KEY("rent_offer_id") REFERENCES "RentOffers"("id") ON UPDATE CASCADE ON DELETE RESTRICT;

CREATE TABLE "Logs"(
    "id" UUID NOT NULL DEFAULT gen_random_uuid(),
    "user_id" UUID NULL,
    "action_type" VARCHAR(100) NOT NULL,
    "entity_type" VARCHAR(100) NOT NULL,
    "entity_id" UUID NOT NULL,
    "old_values" TEXT NULL,
    "new_values" TEXT NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "is_deleted" BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY("id")
);
ALTER TABLE "Logs" ADD CONSTRAINT "fk_logs_user" FOREIGN KEY("user_id") REFERENCES "Users"("id") ON UPDATE CASCADE ON DELETE SET NULL;

COMMIT;