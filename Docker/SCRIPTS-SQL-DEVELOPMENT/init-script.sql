-- CRIANDO USUARIOS
CREATE USER development_user IDENTIFIED BY 171204;
GRANT CONNECT, RESOURCE TO development_user;

-- CONCEDENDO PERMISSOES ENTRE TABELAS DE DIFERENTES SCHEMAS
GRANT UNLIMITED TABLESPACE TO development_user;

-- CRIANDO TABELAS NO ESQUEMA development_user

ALTER SESSION SET CURRENT_SCHEMA = development_user;

--------------------------------------------------------
--  Arquivo criado - quinta-feira-setembro-19-2024   
--------------------------------------------------------

--------------------------------------------------------
--  DDL for Table __EFMigrationsHistory
--------------------------------------------------------

  CREATE TABLE "__EFMigrationsHistory" 
   (	"MigrationId" NVARCHAR2(150), 
	"ProductVersion" NVARCHAR2(32)
   );

--------------------------------------------------------
--  DDL for Table T_GR_AGENDA
--------------------------------------------------------

  CREATE TABLE "T_GR_AGENDA" 
   (	"agenda_id" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"caminhao_id" NUMBER(10,0), 
	"motorista_id" NUMBER(10,0), 
	"bairro_id" NUMBER(10,0), 
	"dia_coleta_de_lixo" NVARCHAR2(10), 
	"tipo_residuo" NVARCHAR2(2000), 
	"status_coleta_de_lixo_agendada" NVARCHAR2(2000), 
	"peso_coletado_de_lixo_kg" NUMBER(19,0)
   );
--------------------------------------------------------
--  DDL for Table T_GR_BAIRRO
--------------------------------------------------------

  CREATE TABLE "T_GR_BAIRRO" 
   (	"bairro_id" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"bairro_nome" NVARCHAR2(450), 
	"quantidade_lixeiras" NUMBER(10,0), 
	"peso_medio_lixeiras_kg" NUMBER(19,0), 
	"percentual_coleta_lixo_bairro" NUMBER(10,0), 
	"bairro_esta_disponivel" NUMBER(10,0)
   );
--------------------------------------------------------
--  DDL for Table T_GR_CAMINHAO
--------------------------------------------------------

  CREATE TABLE "T_GR_CAMINHAO" 
   (	"caminhao_id" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"caminhao_placa" NVARCHAR2(450), 
	"data_fabricacao" NVARCHAR2(10), 
	"caminhao_marca" NVARCHAR2(2000), 
	"caminhao_modelo" NVARCHAR2(2000), 
	"caminhao_esta_disponivel" NUMBER(10,0)
   );
--------------------------------------------------------
--  DDL for Table T_GR_MORADOR
--------------------------------------------------------

  CREATE TABLE "T_GR_MORADOR" 
   (	"morador_id" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"bairro_id" NUMBER(10,0), 
	"morador_nome" NVARCHAR2(2000), 
	"morador_email" NVARCHAR2(450)
   );
--------------------------------------------------------
--  DDL for Table T_GR_MOTORISTA
--------------------------------------------------------

  CREATE TABLE "T_GR_MOTORISTA" 
   (	"motorista_id" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"motorista_nome" NVARCHAR2(2000), 
	"motorista_cpf" NVARCHAR2(450), 
	"motorista_nr_celular" NVARCHAR2(450), 
	"motorista_nr_celular_ddd" NVARCHAR2(2000), 
	"motorista_nr_celular_ddi" NVARCHAR2(2000), 
	"motorista_esta_disponivel" NUMBER(10,0)
   );
--------------------------------------------------------
--  DDL for Table T_GR_NOTIFICACAO
--------------------------------------------------------

  CREATE TABLE "T_GR_NOTIFICACAO" 
   (	"notificacao_id" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"agenda_id" NUMBER(10,0)
   );
--------------------------------------------------------
--  DDL for Table T_GR_USUARIO
--------------------------------------------------------

  CREATE TABLE "T_GR_USUARIO" 
   (	"UsuarioId" NUMBER(10,0) GENERATED BY DEFAULT ON NULL AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"usuario_nome" NVARCHAR2(2000), 
	"usuario_email" NVARCHAR2(450), 
	"usuario_senha" NVARCHAR2(2000), 
	"usuario_role" NVARCHAR2(2000)
   );


--------------------------------------------------------
--  DDL for Index PK___EFMigrationsHistory
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK___EFMigrationsHistory" ON "__EFMigrationsHistory" ("MigrationId");
--------------------------------------------------------
--  DDL for Index PK_T_GR_AGENDA
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_AGENDA" ON "T_GR_AGENDA" ("agenda_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_AGENDA_bairro_id
--------------------------------------------------------

  CREATE INDEX "IX_T_GR_AGENDA_bairro_id" ON "T_GR_AGENDA" ("bairro_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_AGENDA_caminhao_id
--------------------------------------------------------

  CREATE INDEX "IX_T_GR_AGENDA_caminhao_id" ON "T_GR_AGENDA" ("caminhao_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_AGENDA_motorista_id
--------------------------------------------------------

  CREATE INDEX "IX_T_GR_AGENDA_motorista_id" ON "T_GR_AGENDA" ("motorista_id");
--------------------------------------------------------
--  DDL for Index PK_T_GR_BAIRRO
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_BAIRRO" ON "T_GR_BAIRRO" ("bairro_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_BAIRRO_bairro_nome
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_BAIRRO_bairro_nome" ON "T_GR_BAIRRO" ("bairro_nome");
--------------------------------------------------------
--  DDL for Index PK_T_GR_CAMINHAO
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_CAMINHAO" ON "T_GR_CAMINHAO" ("caminhao_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_CAMINHAO_caminhao_placa
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_CAMINHAO_caminhao_placa" ON "T_GR_CAMINHAO" ("caminhao_placa");
--------------------------------------------------------
--  DDL for Index PK_T_GR_MORADOR
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_MORADOR" ON "T_GR_MORADOR" ("morador_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_MORADOR_bairro_id
--------------------------------------------------------

  CREATE INDEX "IX_T_GR_MORADOR_bairro_id" ON "T_GR_MORADOR" ("bairro_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_MORADOR_morador_email
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_MORADOR_morador_email" ON "T_GR_MORADOR" ("morador_email");
--------------------------------------------------------
--  DDL for Index IX_T_GR_MOTORISTA_motorista_cpf
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_MOTORISTA_motorista_cpf" ON "T_GR_MOTORISTA" ("motorista_cpf");
--------------------------------------------------------
--  DDL for Index IX_T_GR_MOTORISTA_motorista_nr_celular
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_MOTORISTA_motorista_nr_celular" ON "T_GR_MOTORISTA" ("motorista_nr_celular");
--------------------------------------------------------
--  DDL for Index PK_T_GR_MOTORISTA
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_MOTORISTA" ON "T_GR_MOTORISTA" ("motorista_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_NOTIFICACAO_agenda_id
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_NOTIFICACAO_agenda_id" ON "T_GR_NOTIFICACAO" ("agenda_id");
--------------------------------------------------------
--  DDL for Index PK_T_GR_NOTIFICACAO
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_NOTIFICACAO" ON "T_GR_NOTIFICACAO" ("notificacao_id");
--------------------------------------------------------
--  DDL for Index IX_T_GR_USUARIO_usuario_email
--------------------------------------------------------

  CREATE UNIQUE INDEX "IX_T_GR_USUARIO_usuario_email" ON "T_GR_USUARIO" ("usuario_email");
--------------------------------------------------------
--  DDL for Index PK_T_GR_USUARIO
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_T_GR_USUARIO" ON "T_GR_USUARIO" ("UsuarioId");

--------------------------------------------------------
--  Constraints for Table __EFMigrationsHistory
--------------------------------------------------------

  ALTER TABLE "__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
  ALTER TABLE "__EFMigrationsHistory" MODIFY ("MigrationId" NOT NULL ENABLE);
  ALTER TABLE "__EFMigrationsHistory" MODIFY ("ProductVersion" NOT NULL ENABLE);

--------------------------------------------------------
--  Constraints for Table T_GR_AGENDA
--------------------------------------------------------

  ALTER TABLE "T_GR_AGENDA" ADD CONSTRAINT "PK_T_GR_AGENDA" PRIMARY KEY ("agenda_id");
  ALTER TABLE "T_GR_AGENDA" MODIFY ("agenda_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("caminhao_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("motorista_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("bairro_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("dia_coleta_de_lixo" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("tipo_residuo" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("status_coleta_de_lixo_agendada" NOT NULL ENABLE);
  ALTER TABLE "T_GR_AGENDA" MODIFY ("peso_coletado_de_lixo_kg" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table T_GR_BAIRRO
--------------------------------------------------------

  ALTER TABLE "T_GR_BAIRRO" ADD CONSTRAINT "PK_T_GR_BAIRRO" PRIMARY KEY ("bairro_id");
  ALTER TABLE "T_GR_BAIRRO" MODIFY ("bairro_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_BAIRRO" MODIFY ("bairro_nome" NOT NULL ENABLE);
  ALTER TABLE "T_GR_BAIRRO" MODIFY ("quantidade_lixeiras" NOT NULL ENABLE);
  ALTER TABLE "T_GR_BAIRRO" MODIFY ("peso_medio_lixeiras_kg" NOT NULL ENABLE);
  ALTER TABLE "T_GR_BAIRRO" MODIFY ("percentual_coleta_lixo_bairro" NOT NULL ENABLE);
  ALTER TABLE "T_GR_BAIRRO" MODIFY ("bairro_esta_disponivel" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table T_GR_CAMINHAO
--------------------------------------------------------

  ALTER TABLE "T_GR_CAMINHAO" ADD CONSTRAINT "PK_T_GR_CAMINHAO" PRIMARY KEY ("caminhao_id");
  ALTER TABLE "T_GR_CAMINHAO" MODIFY ("caminhao_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_CAMINHAO" MODIFY ("caminhao_placa" NOT NULL ENABLE);
  ALTER TABLE "T_GR_CAMINHAO" MODIFY ("data_fabricacao" NOT NULL ENABLE);
  ALTER TABLE "T_GR_CAMINHAO" MODIFY ("caminhao_marca" NOT NULL ENABLE);
  ALTER TABLE "T_GR_CAMINHAO" MODIFY ("caminhao_modelo" NOT NULL ENABLE);
  ALTER TABLE "T_GR_CAMINHAO" MODIFY ("caminhao_esta_disponivel" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table T_GR_MORADOR
--------------------------------------------------------

  ALTER TABLE "T_GR_MORADOR" ADD CONSTRAINT "PK_T_GR_MORADOR" PRIMARY KEY ("morador_id");
  ALTER TABLE "T_GR_MORADOR" MODIFY ("morador_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MORADOR" MODIFY ("bairro_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MORADOR" MODIFY ("morador_nome" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MORADOR" MODIFY ("morador_email" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table T_GR_MOTORISTA
--------------------------------------------------------

  ALTER TABLE "T_GR_MOTORISTA" ADD CONSTRAINT "PK_T_GR_MOTORISTA" PRIMARY KEY ("motorista_id");
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_nome" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_cpf" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_nr_celular" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_nr_celular_ddd" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_nr_celular_ddi" NOT NULL ENABLE);
  ALTER TABLE "T_GR_MOTORISTA" MODIFY ("motorista_esta_disponivel" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table T_GR_NOTIFICACAO
--------------------------------------------------------

  ALTER TABLE "T_GR_NOTIFICACAO" ADD CONSTRAINT "PK_T_GR_NOTIFICACAO" PRIMARY KEY ("notificacao_id");
  ALTER TABLE "T_GR_NOTIFICACAO" MODIFY ("notificacao_id" NOT NULL ENABLE);
  ALTER TABLE "T_GR_NOTIFICACAO" MODIFY ("agenda_id" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table T_GR_USUARIO
--------------------------------------------------------

  ALTER TABLE "T_GR_USUARIO" ADD CONSTRAINT "PK_T_GR_USUARIO" PRIMARY KEY ("UsuarioId");
  ALTER TABLE "T_GR_USUARIO" MODIFY ("UsuarioId" NOT NULL ENABLE);
  ALTER TABLE "T_GR_USUARIO" MODIFY ("usuario_nome" NOT NULL ENABLE);
  ALTER TABLE "T_GR_USUARIO" MODIFY ("usuario_email" NOT NULL ENABLE);
  ALTER TABLE "T_GR_USUARIO" MODIFY ("usuario_senha" NOT NULL ENABLE);
  ALTER TABLE "T_GR_USUARIO" MODIFY ("usuario_role" NOT NULL ENABLE);
--------------------------------------------------------
--  Ref Constraints for Table T_GR_AGENDA
--------------------------------------------------------

  ALTER TABLE "T_GR_AGENDA" ADD CONSTRAINT "FK_T_GR_AGENDA_BAIRRO" FOREIGN KEY ("bairro_id")
	  REFERENCES "T_GR_BAIRRO" ("bairro_id") ENABLE;
  ALTER TABLE "T_GR_AGENDA" ADD CONSTRAINT "FK_T_GR_AGENDA_CAMINHAO" FOREIGN KEY ("caminhao_id")
	  REFERENCES "T_GR_CAMINHAO" ("caminhao_id") ENABLE;
  ALTER TABLE "T_GR_AGENDA" ADD CONSTRAINT "FK_T_GR_AGENDA_MOTORISTA" FOREIGN KEY ("motorista_id")
	  REFERENCES "T_GR_MOTORISTA" ("motorista_id") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table T_GR_MORADOR
--------------------------------------------------------

  ALTER TABLE "T_GR_MORADOR" ADD CONSTRAINT "FK_T_GR_MORADOR_BAIRRO" FOREIGN KEY ("bairro_id")
	  REFERENCES "T_GR_BAIRRO" ("bairro_id") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table T_GR_NOTIFICACAO
--------------------------------------------------------

  ALTER TABLE "T_GR_NOTIFICACAO" ADD CONSTRAINT "FK_T_GR_NOTIFICACAO_AGENDA" FOREIGN KEY ("agenda_id")
	  REFERENCES "T_GR_AGENDA" ("agenda_id") ENABLE;

--------------------------------------------------------
--  DML for Table T_GR_USARIO
--------------------------------------------------------
INSERT INTO "T_GR_USUARIO" 
("usuario_nome","usuario_email","usuario_senha","usuario_role") 
values ('Admin Base Teste','adminbaseteste@gereresiduo.com.br','93f82e739e7418e6107da4051fb5dd8223707e3887aa27dfd32c8855e80d77f7','ADMIN');

--------------------------------------------------------
--  TCL
--------------------------------------------------------

COMMIT;