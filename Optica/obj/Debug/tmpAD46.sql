CREATE SCHEMA dbo
;

CREATE TABLE "dbo"."ActeMedicals"("Id" serial4 NOT NULL,"Code" text,"Designation" text,"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.ActeMedicals" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."Traitements"("Id" serial4 NOT NULL,"PrixAssur" float4 NOT NULL DEFAULT 0,"PrixOrd" float4 NOT NULL DEFAULT 0,"MontantPaye" float4 NOT NULL DEFAULT 0,"PayeOrd" boolean NOT NULL DEFAULT FALSE,"PayeAssur" boolean NOT NULL DEFAULT FALSE,"Resultat" text,"Remarque" text,"Recommandation" text,"DateTrait" timestamp NOT NULL DEFAULT '-infinity',"DateEnregTrait" timestamp NOT NULL DEFAULT '-infinity',"MedecinId" int4 NOT NULL DEFAULT 0,"ActeMedicalId" int4 NOT NULL DEFAULT 0,"AssuranceId" int4 NOT NULL DEFAULT 0,"DossierPatId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.Traitements" PRIMARY KEY ("Id"))
;

CREATE INDEX "Traitements_IX_ActeMedicalId" ON "dbo"."Traitements" ("ActeMedicalId")
;

CREATE INDEX "Traitements_IX_AssuranceId" ON "dbo"."Traitements" ("AssuranceId")
;

CREATE INDEX "Traitements_IX_MedecinId" ON "dbo"."Traitements" ("MedecinId")
;

CREATE INDEX "Traitements_IX_DossierPatId" ON "dbo"."Traitements" ("DossierPatId")
;

CREATE TABLE "dbo"."Assurances"("Id" serial4 NOT NULL,"Code" text,"Nom" text,"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Assurances" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."DossierPats"("Id" serial4 NOT NULL,"Motif" text,"Statut" boolean NOT NULL DEFAULT FALSE,"PatientId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.DossierPats" PRIMARY KEY ("Id"))
;

CREATE INDEX "DossierPats_IX_PatientId" ON "dbo"."DossierPats" ("PatientId")
;

CREATE TABLE "dbo"."ControlePats"("Id" serial4 NOT NULL,"DateControle" timestamp NOT NULL DEFAULT '-infinity',"DateEffectuerControle" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"MedecinId" int4 NOT NULL DEFAULT 0,"DossierPatId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.ControlePats" PRIMARY KEY ("Id"))
;

CREATE INDEX "ControlePats_IX_DossierPatId" ON "dbo"."ControlePats" ("DossierPatId")
;

CREATE INDEX "ControlePats_IX_MedecinId" ON "dbo"."ControlePats" ("MedecinId")
;

CREATE TABLE "dbo"."Medecins"("Id" serial4 NOT NULL,"NumMat" int4 NOT NULL DEFAULT 0,"Nom" text,"Prenoms" text,"Sexe" text,"Titre" text,"DateEntree" timestamp NOT NULL DEFAULT '-infinity',"DateSortie" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Medecins" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."Patients"("Id" serial4 NOT NULL,"NumMat" int4 NOT NULL DEFAULT 0,"Nom" text,"Prenom" text,"Sexe" text,"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Patients" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."PayementOrds"("Id" serial4 NOT NULL,"TypePaye" text,"InfoCheque" text,"MontantPaye" float4 NOT NULL DEFAULT 0,"DatePaye" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,"TraitementId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.PayementOrds" PRIMARY KEY ("Id"))
;

CREATE INDEX "PayementOrds_IX_TraitementId" ON "dbo"."PayementOrds" ("TraitementId")
;

CREATE TABLE "dbo"."PayementAssurs"("Id" serial4 NOT NULL,"TypePaye" text,"InfoCheque" text,"MontantPaye" float4 NOT NULL DEFAULT 0,"DatePaye" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,"TaitementId" int4 NOT NULL DEFAULT 0,"Traitement_Id" int4,CONSTRAINT "PK_dbo.PayementAssurs" PRIMARY KEY ("Id"))
;

CREATE INDEX "PayementAssurs_IX_Traitement_Id" ON "dbo"."PayementAssurs" ("Traitement_Id")
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.ActeMedicals_ActeMedicalId" FOREIGN KEY ("ActeMedicalId") REFERENCES "dbo"."ActeMedicals" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.Assurances_AssuranceId" FOREIGN KEY ("AssuranceId") REFERENCES "dbo"."Assurances" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.Medecins_MedecinId" FOREIGN KEY ("MedecinId") REFERENCES "dbo"."Medecins" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.DossierPats_DossierPatId" FOREIGN KEY ("DossierPatId") REFERENCES "dbo"."DossierPats" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."DossierPats" ADD CONSTRAINT "FK_dbo.DossierPats_dbo.Patients_PatientId" FOREIGN KEY ("PatientId") REFERENCES "dbo"."Patients" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."ControlePats" ADD CONSTRAINT "FK_dbo.ControlePats_dbo.DossierPats_DossierPatId" FOREIGN KEY ("DossierPatId") REFERENCES "dbo"."DossierPats" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."ControlePats" ADD CONSTRAINT "FK_dbo.ControlePats_dbo.Medecins_MedecinId" FOREIGN KEY ("MedecinId") REFERENCES "dbo"."Medecins" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."PayementOrds" ADD CONSTRAINT "FK_dbo.PayementOrds_dbo.Traitements_TraitementId" FOREIGN KEY ("TraitementId") REFERENCES "dbo"."Traitements" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."PayementAssurs" ADD CONSTRAINT "FK_dbo.PayementAssurs_dbo.Traitements_Traitement_Id" FOREIGN KEY ("Traitement_Id") REFERENCES "dbo"."Traitements" ("Id")
;

CREATE TABLE "dbo"."__MigrationHistory"("MigrationId" varchar(150) NOT NULL DEFAULT '',"ContextKey" varchar(300) NOT NULL DEFAULT '',"Model" bytea NOT NULL DEFAULT '',"ProductVersion" varchar(32) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.__MigrationHistory" PRIMARY KEY ("MigrationId","ContextKey"))
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201503281145132_InitialCreate',E'Optica.Migrations.Configuration',decode('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', 'base64'),E'6.0.0-20911')
;

ALTER TABLE "dbo"."Traitements" DROP CONSTRAINT IF EXISTS "FK_dbo.Traitements_dbo.Assurances_AssuranceId"
;

DROP INDEX IF EXISTS dbo."Traitements_IX_AssuranceId"
;

ALTER TABLE "dbo"."DossierPats" RENAME COLUMN "Statut" TO "StatutClosed"
;

ALTER TABLE "dbo"."DossierPats" ADD "DateEnreg" timestamp NOT NULL DEFAULT '-infinity'
;

ALTER TABLE "dbo"."ActeMedicals" ALTER COLUMN "Code" TYPE text
;

ALTER TABLE "dbo"."ActeMedicals" ALTER COLUMN "Code" SET NOT NULL
;

ALTER TABLE "dbo"."ActeMedicals" ALTER COLUMN "Code" DROP DEFAULT
;

ALTER TABLE "dbo"."Traitements" ALTER COLUMN "AssuranceId" TYPE int4
;

ALTER TABLE "dbo"."Traitements" ALTER COLUMN "AssuranceId" DROP NOT NULL
;

ALTER TABLE "dbo"."Traitements" ALTER COLUMN "AssuranceId" DROP DEFAULT
;

ALTER TABLE "dbo"."Assurances" ALTER COLUMN "Code" TYPE text
;

ALTER TABLE "dbo"."Assurances" ALTER COLUMN "Code" SET NOT NULL
;

ALTER TABLE "dbo"."Assurances" ALTER COLUMN "Code" DROP DEFAULT
;

ALTER TABLE "dbo"."Assurances" ALTER COLUMN "Nom" TYPE text
;

ALTER TABLE "dbo"."Assurances" ALTER COLUMN "Nom" SET NOT NULL
;

ALTER TABLE "dbo"."Assurances" ALTER COLUMN "Nom" DROP DEFAULT
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "NumMat" TYPE text
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "NumMat" SET NOT NULL
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "NumMat" DROP DEFAULT
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Nom" TYPE text
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Nom" SET NOT NULL
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Nom" DROP DEFAULT
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Prenoms" TYPE text
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Prenoms" SET NOT NULL
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Prenoms" DROP DEFAULT
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Sexe" TYPE varchar(1)
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Sexe" DROP NOT NULL
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "Sexe" DROP DEFAULT
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "DateEntree" TYPE timestamp
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "DateEntree" DROP NOT NULL
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "DateEntree" DROP DEFAULT
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "DateSortie" TYPE timestamp
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "DateSortie" DROP NOT NULL
;

ALTER TABLE "dbo"."Medecins" ALTER COLUMN "DateSortie" DROP DEFAULT
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "NumMat" TYPE text
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "NumMat" SET NOT NULL
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "NumMat" DROP DEFAULT
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Nom" TYPE text
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Nom" SET NOT NULL
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Nom" DROP DEFAULT
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Prenom" TYPE text
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Prenom" SET NOT NULL
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Prenom" DROP DEFAULT
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Sexe" TYPE varchar(1)
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Sexe" DROP NOT NULL
;

ALTER TABLE "dbo"."Patients" ALTER COLUMN "Sexe" DROP DEFAULT
;

CREATE INDEX "Traitements_IX_AssuranceId" ON "dbo"."Traitements" ("AssuranceId")
;

ALTER TABLE "dbo"."Traitements" ADD CONSTRAINT "FK_dbo.Traitements_dbo.Assurances_AssuranceId" FOREIGN KEY ("AssuranceId") REFERENCES "dbo"."Assurances" ("Id")
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201504030946379_AjoutContraintes',E'Optica.Migrations.Configuration',decode('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', 'base64'),E'6.0.0-20911')
;

ALTER TABLE "dbo"."PayementOrds" DROP CONSTRAINT IF EXISTS "FK_dbo.PayementOrds_dbo.Traitements_TraitementId"
;

ALTER TABLE "dbo"."PayementAssurs" DROP CONSTRAINT IF EXISTS "FK_dbo.PayementAssurs_dbo.Traitements_Traitement_Id"
;

DROP INDEX IF EXISTS dbo."PayementOrds_IX_TraitementId"
;

DROP INDEX IF EXISTS dbo."PayementAssurs_IX_Traitement_Id"
;

CREATE SCHEMA dbo
;

CREATE TABLE "dbo"."Payements"("Id" serial4 NOT NULL,"TypePaye" text NOT NULL DEFAULT '',"InfoCheque" text,"MontantPaye" float4 NOT NULL DEFAULT 0,"DatePaye" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,"SourcePaye" int4 NOT NULL DEFAULT 0,"TraitementId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.Payements" PRIMARY KEY ("Id"))
;

CREATE INDEX "Payements_IX_TraitementId" ON "dbo"."Payements" ("TraitementId")
;

DROP TABLE "dbo"."PayementOrds"
;

DROP TABLE "dbo"."PayementAssurs"
;

ALTER TABLE "dbo"."Payements" ADD CONSTRAINT "FK_dbo.Payements_dbo.Traitements_TraitementId" FOREIGN KEY ("TraitementId") REFERENCES "dbo"."Traitements" ("Id") ON DELETE CASCADE
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201504031016049_AddPayementEntite',E'Optica.Migrations.Configuration',decode('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', 'base64'),E'6.0.0-20911')
