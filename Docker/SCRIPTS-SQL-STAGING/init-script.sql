-- CRIANDO USUARIOS
CREATE USER staging_user IDENTIFIED BY 171204;
GRANT CONNECT, RESOURCE TO staging_user;

-- CONCEDENDO PERMISSOES ENTRE TABELAS DE DIFERENTES SCHEMAS
GRANT UNLIMITED TABLESPACE TO staging_user;

-- CRIANDO TABELAS NO ESQUEMA staging_user

ALTER SESSION SET CURRENT_SCHEMA = staging_user;

-- Criar a tabela T_GR_BAIRRO
CREATE TABLE T_GR_BAIRRO (
    bairro_id NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    bairro_nome NVARCHAR2(450) NOT NULL,
    quantidade_lixeiras NUMBER(10) NOT NULL,
    peso_medio_lixeiras_kg NUMBER(19) NOT NULL,
    percentual_coleta_lixo_bairro NUMBER(10) NOT NULL,
    bairro_esta_disponivel NUMBER(10) NOT NULL,
    CONSTRAINT PK_T_GR_BAIRRO PRIMARY KEY (bairro_id)
);

-- Criar a tabela T_GR_CAMINHAO
CREATE TABLE T_GR_CAMINHAO (
    caminhao_id NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    caminhao_placa NVARCHAR2(450) NOT NULL,
    data_fabricacao NVARCHAR2(10) NOT NULL,
    caminhao_marca NVARCHAR2(2000) NOT NULL,
    caminhao_modelo NVARCHAR2(2000) NOT NULL,
    caminhao_esta_disponivel NUMBER(10) NOT NULL,
    CONSTRAINT PK_T_GR_CAMINHAO PRIMARY KEY (caminhao_id)
);

-- Criar a tabela T_GR_MOTORISTA
CREATE TABLE T_GR_MOTORISTA (
    motorista_id NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    motorista_nome NVARCHAR2(2000) NOT NULL,
    motorista_cpf NVARCHAR2(450) NOT NULL,
    motorista_nr_celular NVARCHAR2(450) NOT NULL,
    motorista_nr_celular_ddd NVARCHAR2(2000) NOT NULL,
    motorista_nr_celular_ddi NVARCHAR2(2000) NOT NULL,
    motorista_esta_disponivel NUMBER(10) NOT NULL,
    CONSTRAINT PK_T_GR_MOTORISTA PRIMARY KEY (motorista_id)
);

-- Criar a tabela T_GR_USUARIO
CREATE TABLE T_GR_USUARIO (
    UsuarioId NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    usuario_nome NVARCHAR2(2000) NOT NULL,
    usuario_email NVARCHAR2(450) NOT NULL,
    usuario_senha NVARCHAR2(2000) NOT NULL,
    usuario_role NVARCHAR2(2000) NOT NULL,
    CONSTRAINT PK_T_GR_USUARIO PRIMARY KEY (UsuarioId)
);

-- Criar a tabela T_GR_MORADOR
CREATE TABLE T_GR_MORADOR (
    morador_id NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    bairro_id NUMBER(10) NOT NULL,
    morador_nome NVARCHAR2(2000) NOT NULL,
    morador_email NVARCHAR2(450) NOT NULL,
    CONSTRAINT PK_T_GR_MORADOR PRIMARY KEY (morador_id),
    CONSTRAINT FK_T_GR_MORADOR_BAIRRO FOREIGN KEY (bairro_id) REFERENCES T_GR_BAIRRO (bairro_id)
);

-- Criar a tabela T_GR_AGENDA
CREATE TABLE T_GR_AGENDA (
    agenda_id NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    caminhao_id NUMBER(10) NOT NULL,
    motorista_id NUMBER(10) NOT NULL,
    bairro_id NUMBER(10) NOT NULL,
    dia_coleta_de_lixo NVARCHAR2(10) NOT NULL,
    tipo_residuo NVARCHAR2(2000) NOT NULL,
    status_coleta_de_lixo_agendada NVARCHAR2(2000) NOT NULL,
    peso_coletado_de_lixo_kg NUMBER(19) NOT NULL,
    CONSTRAINT PK_T_GR_AGENDA PRIMARY KEY (agenda_id),
    CONSTRAINT FK_T_GR_AGENDA_BAIRRO FOREIGN KEY (bairro_id) REFERENCES T_GR_BAIRRO (bairro_id),
    CONSTRAINT FK_T_GR_AGENDA_CAMINHAO FOREIGN KEY (caminhao_id) REFERENCES T_GR_CAMINHAO (caminhao_id),
    CONSTRAINT FK_T_GR_AGENDA_MOTORISTA FOREIGN KEY (motorista_id) REFERENCES T_GR_MOTORISTA (motorista_id)
);

-- Criar a tabela T_GR_NOTIFICACAO
CREATE TABLE T_GR_NOTIFICACAO (
    notificacao_id NUMBER(10) GENERATED BY DEFAULT AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    agenda_id NUMBER(10) NOT NULL,
    CONSTRAINT PK_T_GR_NOTIFICACAO PRIMARY KEY (notificacao_id),
    CONSTRAINT FK_T_GR_NOTIFICACAO_AGENDA FOREIGN KEY (agenda_id) REFERENCES T_GR_AGENDA (agenda_id)
);

-- Criar índices únicos
CREATE UNIQUE INDEX IX_T_GR_BAIRRO_bairro_nome ON T_GR_BAIRRO (bairro_nome);
CREATE UNIQUE INDEX IX_T_GR_CAMINHAO_caminhao_placa ON T_GR_CAMINHAO (caminhao_placa);
CREATE UNIQUE INDEX IX_T_GR_MORADOR_morador_email ON T_GR_MORADOR (morador_email);
CREATE UNIQUE INDEX IX_T_GR_MOTORISTA_motorista_cpf ON T_GR_MOTORISTA (motorista_cpf);
CREATE UNIQUE INDEX IX_T_GR_MOTORISTA_motorista_nr_celular ON T_GR_MOTORISTA (motorista_nr_celular);
CREATE UNIQUE INDEX IX_T_GR_NOTIFICACAO_agenda_id ON T_GR_NOTIFICACAO (agenda_id);
CREATE UNIQUE INDEX IX_T_GR_USUARIO_usuario_email ON T_GR_USUARIO (usuario_email);

-- Criar índices não únicos
CREATE INDEX IX_T_GR_AGENDA_bairro_id ON T_GR_AGENDA (bairro_id);
CREATE INDEX IX_T_GR_AGENDA_caminhao_id ON T_GR_AGENDA (caminhao_id);
CREATE INDEX IX_T_GR_AGENDA_motorista_id ON T_GR_AGENDA (motorista_id);
CREATE INDEX IX_T_GR_MORADOR_bairro_id ON T_GR_MORADOR (bairro_id);