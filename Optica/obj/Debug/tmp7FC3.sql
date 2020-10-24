ALTER TABLE "dbo"."Traitements" DROP CONSTRAINT IF EXISTS "FK_dbo.Traitements_dbo.Medecins_MedecinId"
;

DROP INDEX IF EXISTS dbo."Traitements_IX_MedecinId"
;

ALTER TABLE "dbo"."Traitements" RENAME COLUMN "MedecinId" TO "MedecinTraitant_Id"
;

ALTER TABLE "dbo"."Traitements" ADD "MedecinRecommandant_Id" int4
;

ALTER TABLE "dbo"."Traitements" ALTER COLUMN "MedecinTraitant_Id" TYPE int4
;

ALTER TABLE "dbo"."Traitements" ALTER COLUMN "MedecinTraitant_Id" DROP NOT NULL
;

ALTER TABLE "dbo"."Traitements" ALTER COLUMN "MedecinTraitant_Id" DROP DEFAULT
;

CREATE INDEX "Traitements_IX_MedecinTraitant_Id" ON "dbo"."Traitements" ("MedecinTraitant_Id")
;

CREATE INDEX "Traitements_IX_MedecinRecommandant_Id" ON "dbo"."Traitements" ("MedecinRecommandant_Id")
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.Medecins_MedecinRecommandant_Id" FOREIGN KEY ("MedecinRecommandant_Id") REFERENCES "dbo"."Medecins" ("Id")
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.Medecins_MedecinTraitant_Id" FOREIGN KEY ("MedecinTraitant_Id") REFERENCES "dbo"."Medecins" ("Id")
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201601181300183_MiseAjourRelationMedecinTraitement',E'Optica.Migrations.Configuration',decode('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', 'base64'),E'6.1.3-40302')
