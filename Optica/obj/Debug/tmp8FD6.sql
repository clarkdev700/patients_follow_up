CREATE SCHEMA dbo
;

CREATE TABLE "dbo"."AspNetRoles"("Id" varchar(128) NOT NULL DEFAULT '',"Name" text NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetRoles" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."AspNetUsers"("Id" varchar(128) NOT NULL DEFAULT '',"UserName" text,"PasswordHash" text,"SecurityStamp" text,"Discriminator" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUsers" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."AspNetUserClaims"("Id" serial4 NOT NULL,"ClaimType" text,"ClaimValue" text,"User_Id" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUserClaims" PRIMARY KEY ("Id"))
;

CREATE INDEX "AspNetUserClaims_IX_User_Id" ON "dbo"."AspNetUserClaims" ("User_Id")
;

CREATE TABLE "dbo"."AspNetUserLogins"("UserId" varchar(128) NOT NULL DEFAULT '',"LoginProvider" varchar(128) NOT NULL DEFAULT '',"ProviderKey" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUserLogins" PRIMARY KEY ("UserId","LoginProvider","ProviderKey"))
;

CREATE INDEX "AspNetUserLogins_IX_UserId" ON "dbo"."AspNetUserLogins" ("UserId")
;

CREATE TABLE "dbo"."AspNetUserRoles"("UserId" varchar(128) NOT NULL DEFAULT '',"RoleId" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUserRoles" PRIMARY KEY ("UserId","RoleId"))
;

CREATE INDEX "AspNetUserRoles_IX_RoleId" ON "dbo"."AspNetUserRoles" ("RoleId")
;

CREATE INDEX "AspNetUserRoles_IX_UserId" ON "dbo"."AspNetUserRoles" ("UserId")
;

ALTER TABLE "dbo"."AspNetUserClaims" ADD CONSTRAINT "FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id" FOREIGN KEY ("User_Id") REFERENCES "dbo"."AspNetUsers" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AspNetUserLogins" ADD CONSTRAINT "FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "dbo"."AspNetUsers" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AspNetUserRoles" ADD CONSTRAINT "FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "dbo"."AspNetRoles" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AspNetUserRoles" ADD CONSTRAINT "FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "dbo"."AspNetUsers" ("Id") ON DELETE CASCADE
;

CREATE TABLE "dbo"."__MigrationHistory"("MigrationId" varchar(150) NOT NULL DEFAULT '',"ContextKey" varchar(300) NOT NULL DEFAULT '',"Model" bytea NOT NULL DEFAULT '',"ProductVersion" varchar(32) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.__MigrationHistory" PRIMARY KEY ("MigrationId","ContextKey"))
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201504151655572_Init',E'Optica.IdentityMigrations.Configuration',decode('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', 'base64'),E'6.0.0-20911')
