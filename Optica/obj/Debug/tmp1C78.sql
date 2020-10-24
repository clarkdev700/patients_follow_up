ALTER TABLE "dbo"."ControlePats" ALTER COLUMN "DateEffectuerControle" TYPE timestamp
;

ALTER TABLE "dbo"."ControlePats" ALTER COLUMN "DateEffectuerControle" SET NOT NULL
;

ALTER TABLE "dbo"."ControlePats" ALTER COLUMN "DateEffectuerControle" DROP DEFAULT
;

ALTER TABLE "dbo"."Patients" DROP COLUMN "Email"
;

ALTER TABLE "dbo"."Patients" DROP COLUMN "Contact"
;

ALTER TABLE "dbo"."Medecins" DROP COLUMN "Contact"
;

DELETE FROM "dbo"."__MigrationHistory" WHERE "MigrationId" = E'201504131645166_ContactPatient_et_Medecin' AND "ContextKey" = E'Optica.Migrations.Configuration'
