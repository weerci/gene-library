----------------------------------------------
-- Export file for user MODERN              --
-- Created by oxana on 19.11.2009, 21:34:14 --
----------------------------------------------

spool modern.log

prompt
prompt Creating table POST
prompt ===================
prompt
create table MODERN.POST
(
  ID   NUMBER(10) not null,
  NAME NVARCHAR2(50) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.POST
  add constraint PK_POST primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.POST_NAME_IDX on MODERN.POST (NAME)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table DIVISION
prompt =======================
prompt
create table MODERN.DIVISION
(
  ID      NUMBER(10) not null,
  NAME    NVARCHAR2(50) not null,
  ADDRESS NVARCHAR2(500),
  PHONE   NVARCHAR2(20)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.DIVISION
  add constraint PK_DIVISION primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.DIV_NAME_IDX on MODERN.DIVISION (NAME)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table EXPERT
prompt =====================
prompt
create table MODERN.EXPERT
(
  ID          NUMBER(10) not null,
  DIVISION_ID NUMBER(10) not null,
  POST_ID     NUMBER(10) not null,
  SURNAME     NVARCHAR2(50) not null,
  NAME        NVARCHAR2(50) not null,
  PATRONIC    NVARCHAR2(50) not null,
  LOGIN       NVARCHAR2(20) not null,
  PASSWORD    NVARCHAR2(512),
  SIGN        NVARCHAR2(32),
  SALT        NVARCHAR2(4) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.EXPERT
  add constraint PK_EXPERT primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.EXPERT
  add constraint EXP_DIV_FK foreign key (DIVISION_ID)
  references MODERN.DIVISION (ID);
alter table MODERN.EXPERT
  add constraint EXP_POST_FK foreign key (POST_ID)
  references MODERN.POST (ID);
create index MODERN.EXP_DIV_FX on MODERN.EXPERT (DIVISION_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.EXP_POST_FX on MODERN.EXPERT (POST_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table PROTECT_FUNCTION
prompt ===============================
prompt
create table MODERN.PROTECT_FUNCTION
(
  ID      NUMBER(10) not null,
  NAME    NVARCHAR2(32) not null,
  NOTE    NVARCHAR2(1024),
  CAPTION NVARCHAR2(128) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.PROTECT_FUNCTION
  add constraint PK_PROTECT_FUNCTION primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table ACTION_EXPET
prompt ===========================
prompt
create table MODERN.ACTION_EXPET
(
  FUNC_ID    NUMBER(10) not null,
  EXPERT_ID  NUMBER(10) not null,
  PERMISSION NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ACTION_EXPET
  add constraint PK_ACTION_EXPET primary key (FUNC_ID, EXPERT_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ACTION_EXPET
  add constraint ACT_EXP_ACT_FK foreign key (FUNC_ID)
  references MODERN.PROTECT_FUNCTION (ID);
alter table MODERN.ACTION_EXPET
  add constraint RES_EXP_EXPERT_FK foreign key (EXPERT_ID)
  references MODERN.EXPERT (ID);
alter table MODERN.ACTION_EXPET
  add constraint CKC_PERMISSION_ACTION_E
  check (PERMISSION in (-1,1));

prompt
prompt Creating table CONTROLS
prompt =======================
prompt
create table MODERN.CONTROLS
(
  ID        NUMBER(10) not null,
  NAME      NVARCHAR2(32) not null,
  FUNC_ID   NUMBER(10) not null,
  PARENT_ID NUMBER(10),
  CAPTION   NVARCHAR2(128) not null,
  SORT_ORD  NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CONTROLS
  add constraint PK_CONTROLS primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CONTROLS
  add constraint RES_FUNC_FK foreign key (FUNC_ID)
  references MODERN.PROTECT_FUNCTION (ID);
alter table MODERN.CONTROLS
  add constraint RES_RES_FK foreign key (PARENT_ID)
  references MODERN.CONTROLS (ID);
create index MODERN.CONTROL_FUNC_IDX on MODERN.CONTROLS (FUNC_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.CONTROL_PARENT_IDX on MODERN.CONTROLS (PARENT_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table GROUPS
prompt =====================
prompt
create table MODERN.GROUPS
(
  ID   NUMBER(10) not null,
  NAME NVARCHAR2(128) not null,
  NOTE NVARCHAR2(1024)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.GROUPS
  add constraint PK_GROUPS primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table ACTION_GROUP
prompt ===========================
prompt
create table MODERN.ACTION_GROUP
(
  ID      NUMBER(10) not null,
  RES_ID  NUMBER(10) not null,
  FUNC_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ACTION_GROUP
  add constraint PK_ACTION_GROUP primary key (ID, RES_ID, FUNC_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ACTION_GROUP
  add constraint ACT_GROUP_FUNC_FK foreign key (FUNC_ID)
  references MODERN.PROTECT_FUNCTION (ID);
alter table MODERN.ACTION_GROUP
  add constraint ACT_GROUP_GROUP_FK foreign key (ID)
  references MODERN.GROUPS (ID);
alter table MODERN.ACTION_GROUP
  add constraint ACT_GROUP_RES_FK foreign key (RES_ID)
  references MODERN.CONTROLS (ID);

prompt
prompt Creating table ROLES
prompt ====================
prompt
create table MODERN.ROLES
(
  ID   NUMBER(10) not null,
  NAME NVARCHAR2(32) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ROLES
  add constraint PK_ROLES primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table ACTION_ROLES
prompt ===========================
prompt
create table MODERN.ACTION_ROLES
(
  ROLE_ID NUMBER(10) not null,
  FUNC_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ACTION_ROLES
  add constraint PK_ACTION_ROLES primary key (ROLE_ID, FUNC_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ACTION_ROLES
  add constraint ACT_ROLE_ACT_FK foreign key (FUNC_ID)
  references MODERN.PROTECT_FUNCTION (ID);
alter table MODERN.ACTION_ROLES
  add constraint RES_ROLE_ROLE_FK foreign key (ROLE_ID)
  references MODERN.ROLES (ID);

prompt
prompt Creating table ADJUSTMENT
prompt =========================
prompt
create table MODERN.ADJUSTMENT
(
  ID          NUMBER(10) not null,
  EXPERT_ID   NUMBER(10) not null,
  TAB         NVARCHAR2(128) not null,
  NOTE        NVARCHAR2(512),
  COLUMN_NAME NVARCHAR2(128) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ADJUSTMENT
  add constraint PK_ADJUSTMENT primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ADJUSTMENT
  add constraint ADJ_EXCPERT_FK foreign key (EXPERT_ID)
  references MODERN.EXPERT (ID);
create index MODERN.ADJ_EXP_FX on MODERN.ADJUSTMENT (EXPERT_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.COLUMN_TAB_IDX on MODERN.ADJUSTMENT (COLUMN_NAME, TAB)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table HISTORY_ACTION
prompt =============================
prompt
create table MODERN.HISTORY_ACTION
(
  ID   NUMBER(10) not null,
  NAME NVARCHAR2(512) not null,
  NOTE NVARCHAR2(1024)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.HISTORY_ACTION
  add constraint PK_HISTORY_ACTION primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table HISTORY
prompt ======================
prompt
create table MODERN.HISTORY
(
  ID          NUMBER(10) not null,
  EXPERT_ID   NUMBER(10) not null,
  ACTION_ID   NUMBER(10) not null,
  PARENT_ID   NUMBER(10),
  DATE_INSERT TIMESTAMP(6) not null,
  NOTE        NCLOB
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 6M
    minextents 1
    maxextents unlimited
  );
alter table MODERN.HISTORY
  add constraint PK_HISTORY primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.HISTORY
  add constraint HIST_HIST_FK foreign key (PARENT_ID)
  references MODERN.HISTORY (ID);
alter table MODERN.HISTORY
  add constraint HISTORY_ACTION_FK foreign key (ACTION_ID)
  references MODERN.HISTORY_ACTION (ID);
create index MODERN.HISTORY_ACTION_FX on MODERN.HISTORY (ACTION_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table LOCUS
prompt ====================
prompt
create table MODERN.LOCUS
(
  ID         NUMBER(10) not null,
  NAME       NVARCHAR2(50) not null,
  HISTORY_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.LOCUS
  add constraint PK_LOCUS primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.LOCUS
  add constraint LOCUS_HISTORY_FK foreign key (HISTORY_ID)
  references MODERN.HISTORY (ID);

prompt
prompt Creating table ALLELE
prompt =====================
prompt
create table MODERN.ALLELE
(
  ID            NUMBER(10) not null,
  LOCUS_ID      NUMBER(10) not null,
  NAME          NVARCHAR2(5) not null,
  VAL           NUMBER(4,1) not null,
  HISTORY_ID    NUMBER(10) not null,
  OLD_ALLELE_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ALLELE
  add constraint PK_ALLELE primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ALLELE
  add constraint ALLELE_HISTORY_FK foreign key (HISTORY_ID)
  references MODERN.HISTORY (ID);
alter table MODERN.ALLELE
  add constraint ALL_LOC_FK foreign key (LOCUS_ID)
  references MODERN.LOCUS (ID);
create index MODERN.LOCUS_FX on MODERN.ALLELE (LOCUS_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.OLD_ALLELE_IDX on MODERN.ALLELE (OLD_ALLELE_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table METHOD
prompt =====================
prompt
create table MODERN.METHOD
(
  ID          NUMBER(10) not null,
  NAME        NVARCHAR2(100) not null,
  DEF_FREQ    NUMBER not null,
  DESCRIPTION NVARCHAR2(1024)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.METHOD
  add constraint PK_METHOD primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table ALLELE_FREQ
prompt ==========================
prompt
create table MODERN.ALLELE_FREQ
(
  ALLELE_ID NUMBER(10) not null,
  METHOD_ID NUMBER(10) not null,
  FREQ      NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ALLELE_FREQ
  add constraint PK_ALLELE_FREQ primary key (ALLELE_ID, METHOD_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ALLELE_FREQ
  add constraint ALLFREQ_ALLELE_FK foreign key (ALLELE_ID)
  references MODERN.ALLELE (ID);
alter table MODERN.ALLELE_FREQ
  add constraint ALLFREQ_METHOD_FK foreign key (METHOD_ID)
  references MODERN.METHOD (ID);

prompt
prompt Creating table ORGANIZATION
prompt ===========================
prompt
create table MODERN.ORGANIZATION
(
  ID   NUMBER(10) not null,
  NOTE NVARCHAR2(1024) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ORGANIZATION
  add constraint PK_ORGANIZATION primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CARD
prompt ===================
prompt
create table MODERN.CARD
(
  ID         NUMBER(10) not null,
  CRIM_NUM   NVARCHAR2(100),
  ORG_ID     NUMBER(10) not null,
  EXPERT_ID  NUMBER(10) not null,
  EXAM_NUM   NVARCHAR2(100),
  EXAM_DATE  TIMESTAMP(6),
  EXAM_NOTE  NVARCHAR2(1024),
  DATE_INS   TIMESTAMP(6) not null,
  PAREN_ID   NUMBER(10),
  HISTORY_ID NUMBER(10) not null,
  CARD_NUM   NVARCHAR2(100) not null,
  HASH       NVARCHAR2(512)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 832K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD
  add constraint PK_CARD primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD
  add constraint CARD_CARD_FK foreign key (PAREN_ID)
  references MODERN.CARD (ID);
alter table MODERN.CARD
  add constraint CARD_EXPERT_FK foreign key (EXPERT_ID)
  references MODERN.EXPERT (ID);
alter table MODERN.CARD
  add constraint CARD_HISTORY_FK foreign key (HISTORY_ID)
  references MODERN.HISTORY (ID);
alter table MODERN.CARD
  add constraint CARD_TEXT_FK foreign key (ORG_ID)
  references MODERN.ORGANIZATION (ID);
create index MODERN.CARD_CARD_FX on MODERN.CARD (PAREN_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.CARD_NUMBER_IDX on MODERN.CARD (CARD_NUM)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 192K
    minextents 1
    maxextents unlimited
  );
create index MODERN.EXPERT_FX on MODERN.CARD (EXPERT_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
create index MODERN.HISTORY_FX on MODERN.CARD (HISTORY_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
create index MODERN.ORG_FX on MODERN.CARD (ORG_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CARD_ARCH
prompt ========================
prompt
create table MODERN.CARD_ARCH
(
  ID         NUMBER(10) not null,
  CRIM_NUM   NVARCHAR2(100),
  ORG_ID     NUMBER(10) not null,
  EXPERT_ID  NUMBER(10) not null,
  EXAM_NUM   NVARCHAR2(100),
  EXAM_DATE  TIMESTAMP(6),
  EXAM_NOTE  NVARCHAR2(1024),
  DATE_INS   TIMESTAMP(6) not null,
  PAREN_ID   NUMBER(10),
  HISTORY_ID NUMBER(10) not null,
  CARD_NUM   NVARCHAR2(100) not null,
  HASH       NVARCHAR2(512)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_ARCH
  add constraint PK_CARD_ARCH primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_ARCH
  add constraint CARD_ARCH_EXPERT_FK foreign key (EXPERT_ID)
  references MODERN.EXPERT (ID);
alter table MODERN.CARD_ARCH
  add constraint CARD_ARCH_ORGAN_FK foreign key (ORG_ID)
  references MODERN.ORGANIZATION (ID);
create index MODERN.CARD_CARD_FX2 on MODERN.CARD_ARCH (PAREN_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.CARD_NUMBER_IDX2 on MODERN.CARD_ARCH (CARD_NUM)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.EXPERT_FX2 on MODERN.CARD_ARCH (EXPERT_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.HISTORY_FX2 on MODERN.CARD_ARCH (HISTORY_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.ORG_FX2 on MODERN.CARD_ARCH (ORG_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CARD_IDENT
prompt =========================
prompt
create table MODERN.CARD_IDENT
(
  CARD_ID NUMBER(10) not null,
  ID      NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_IDENT
  add constraint PK_CARD_IDENT primary key (CARD_ID, ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_IDENT
  add constraint CARD_CARD_IDENT_FK foreign key (CARD_ID)
  references MODERN.CARD (ID);

prompt
prompt Creating table CARD_IDENT_ARCH
prompt ==============================
prompt
create table MODERN.CARD_IDENT_ARCH
(
  CARD_ID NUMBER(10) not null,
  ID      NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_IDENT_ARCH
  add constraint PK_CARD_IDENT_ARCH primary key (CARD_ID, ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_IDENT_ARCH
  add constraint CARD_ARCH_CARD_IDENT_ARCH_FK foreign key (CARD_ID)
  references MODERN.CARD_ARCH (ID);

prompt
prompt Creating table UKITEM
prompt =====================
prompt
create table MODERN.UKITEM
(
  ID        NUMBER(10) not null,
  PARENT_ID NUMBER(10),
  HASH      RAW(512) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.UKITEM
  add constraint PK_UKITEM primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.UKITEM
  add constraint UKA_UKA_FK foreign key (PARENT_ID)
  references MODERN.UKITEM (ID);
create index MODERN.HASH_UX on MODERN.UKITEM (HASH)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.UKA_UKA_FX on MODERN.UKITEM (PARENT_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CARD_UKITEM
prompt ==========================
prompt
create table MODERN.CARD_UKITEM
(
  CARD_ID   NUMBER(10) not null,
  UKITEM_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_UKITEM
  add constraint PK_CARD_UKITEM primary key (CARD_ID, UKITEM_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_UKITEM
  add constraint UKITEM_CARD_FK foreign key (CARD_ID)
  references MODERN.CARD (ID);
alter table MODERN.CARD_UKITEM
  add constraint UKITEM_ITEM_FK foreign key (UKITEM_ID)
  references MODERN.UKITEM (ID);

prompt
prompt Creating table CARD_UKITEM_ARCH
prompt ===============================
prompt
create table MODERN.CARD_UKITEM_ARCH
(
  CARD_ID   NUMBER(10) not null,
  UKITEM_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_UKITEM_ARCH
  add constraint PK_CARD_UKITEM_ARCH primary key (CARD_ID, UKITEM_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CARD_UKITEM_ARCH
  add constraint CARD_ARCH_UK_ITEM_ARCH_FK foreign key (CARD_ID)
  references MODERN.CARD_ARCH (ID);
alter table MODERN.CARD_UKITEM_ARCH
  add constraint UKITEM_CARD_UKITEM_ARCH_FK foreign key (UKITEM_ID)
  references MODERN.UKITEM (ID);

prompt
prompt Creating table PROFILE
prompt ======================
prompt
create table MODERN.PROFILE
(
  ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.PROFILE
  add constraint PK_PROFILE primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.PROFILE
  add constraint PROFILE_CARD_FK foreign key (ID)
  references MODERN.CARD (ID);

prompt
prompt Creating table CHK_ALLELE
prompt =========================
prompt
create table MODERN.CHK_ALLELE
(
  PROFILE_ID NUMBER(10) not null,
  LOCUS_ID   NUMBER(10) not null,
  ALLELE_ID  NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 2M
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CHK_ALLELE
  add constraint PK_CHK_ALLELE primary key (PROFILE_ID, LOCUS_ID, ALLELE_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 3M
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CHK_ALLELE
  add constraint CHKALL_ALLELE_FK foreign key (ALLELE_ID)
  references MODERN.ALLELE (ID);
alter table MODERN.CHK_ALLELE
  add constraint FK_CHK_ALLE_REFERENCE_LOCUS foreign key (LOCUS_ID)
  references MODERN.LOCUS (ID);
alter table MODERN.CHK_ALLELE
  add constraint FK_CHK_ALLE_REFERENCE_PROFILE foreign key (PROFILE_ID)
  references MODERN.PROFILE (ID);

prompt
prompt Creating table PROFILE_ARCH
prompt ===========================
prompt
create table MODERN.PROFILE_ARCH
(
  ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.PROFILE_ARCH
  add constraint PK_PROFILE_ARCH primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.PROFILE_ARCH
  add constraint CARD_ARCH_PROFILE_ARCH_FK foreign key (ID)
  references MODERN.CARD_ARCH (ID);

prompt
prompt Creating table CHK_ALLELE_ARCH
prompt ==============================
prompt
create table MODERN.CHK_ALLELE_ARCH
(
  PROFILE_ID NUMBER(10) not null,
  LOCUS_ID   NUMBER(10) not null,
  ALLELE_ID  NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CHK_ALLELE_ARCH
  add constraint PK_CHK_ALLELE_ARCH primary key (PROFILE_ID, LOCUS_ID, ALLELE_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CHK_ALLELE_ARCH
  add constraint ALLELE_CHK_ALLELE_ARCH_FK foreign key (ALLELE_ID)
  references MODERN.ALLELE (ID);
alter table MODERN.CHK_ALLELE_ARCH
  add constraint LOCUS_CHK_ALLELE_ARCH_FK foreign key (LOCUS_ID)
  references MODERN.LOCUS (ID);
alter table MODERN.CHK_ALLELE_ARCH
  add constraint PROFILE_ARCH_CHK_ALLELE_FK foreign key (PROFILE_ID)
  references MODERN.PROFILE_ARCH (ID);

prompt
prompt Creating table CLASS_IKL
prompt ========================
prompt
create table MODERN.CLASS_IKL
(
  ID         NUMBER(10) not null,
  NAME       NVARCHAR2(128) not null,
  SHORT_NAME NVARCHAR2(64)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CLASS_IKL
  add constraint PK_CLASS_IKL primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CLASS_OBJECT
prompt ===========================
prompt
create table MODERN.CLASS_OBJECT
(
  ID         NUMBER(10) not null,
  NAME       NVARCHAR2(128) not null,
  SHORT_NAME NVARCHAR2(64)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CLASS_OBJECT
  add constraint PK_CLASS_OBJECT primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CODE_LIN
prompt =======================
prompt
create table MODERN.CODE_LIN
(
  ID    NUMBER(10) not null,
  CODE  NVARCHAR2(100) not null,
  ORGAN NVARCHAR2(500) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CODE_LIN
  add constraint PK_CODE_LIN primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.CODE_LIN_IDX on MODERN.CODE_LIN (CODE)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CODE_MVD
prompt =======================
prompt
create table MODERN.CODE_MVD
(
  ID         NUMBER(10) not null,
  NAME       NVARCHAR2(100) not null,
  SHORT_NAME NVARCHAR2(100) not null,
  CODE       NVARCHAR2(4) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CODE_MVD
  add constraint PK_CODE_MVD primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.CODE_MVD_IDX on MODERN.CODE_MVD (CODE)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CORRECT_UK
prompt =========================
prompt
create table MODERN.CORRECT_UK
(
  ID     NUMBER(10) not null,
  OLD    NVARCHAR2(100) not null,
  STATE1 NVARCHAR2(100) not null,
  STATE2 NVARCHAR2(100),
  STATE3 NVARCHAR2(100),
  STATE4 NVARCHAR2(100),
  CHK    NUMBER(1) default 0 not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 256K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CORRECT_UK
  add constraint PK_CORRECT_UK primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.CORRECT_UK
  add constraint CORRECT_CARD_FK foreign key (ID)
  references MODERN.CARD (ID);

prompt
prompt Creating table FILTERS
prompt ======================
prompt
create table MODERN.FILTERS
(
  ID   NUMBER(10) not null,
  NAME VARCHAR2(256),
  TEXT NCLOB
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.FILTERS
  add constraint PK_FILTERS primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table IKL
prompt ==================
prompt
create table MODERN.IKL
(
  ID        NUMBER(10) not null,
  PERSON    NVARCHAR2(100) not null,
  ANCILLARY NVARCHAR2(1024),
  CLASS_ID  NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 384K
    minextents 1
    maxextents unlimited
  );
comment on table MODERN.IKL
  is 'Карточка личности';
alter table MODERN.IKL
  add constraint PK_IKL primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.IKL
  add constraint IKL_CARD_FK foreign key (ID)
  references MODERN.CARD (ID);
alter table MODERN.IKL
  add constraint IKL_CLASS_FK foreign key (CLASS_ID)
  references MODERN.CLASS_IKL (ID);

prompt
prompt Creating table IKL_ARCH
prompt =======================
prompt
create table MODERN.IKL_ARCH
(
  ID        NUMBER(10) not null,
  PERSON    NVARCHAR2(100) not null,
  ANCILLARY NVARCHAR2(1024),
  CLASS_ID  NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.IKL_ARCH
  add constraint PK_IKL_ARCH primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.IKL_ARCH
  add constraint IKL_ARCH_CARD_FK foreign key (ID)
  references MODERN.CARD_ARCH (ID);
alter table MODERN.IKL_ARCH
  add constraint IKL_ARCH_CLASS_FK foreign key (CLASS_ID)
  references MODERN.CLASS_IKL (ID);

prompt
prompt Creating table SORT_OBJECT
prompt ==========================
prompt
create table MODERN.SORT_OBJECT
(
  ID         NUMBER(10) not null,
  NAME       NVARCHAR2(128) not null,
  SHORT_NAME NVARCHAR2(64)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.SORT_OBJECT
  add constraint PK_SORT_OBJECT primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table IK2
prompt ==================
prompt
create table MODERN.IK2
(
  ID            NUMBER(10) not null,
  MVD_ID        NUMBER(10) not null,
  LIN_ID        NUMBER(10) not null,
  YEAR_STATE    NUMBER not null,
  ADDRESS_CRIME NVARCHAR2(1024),
  VICTIM        NVARCHAR2(100),
  SN_DNA        NVARCHAR2(32),
  OBJ           NVARCHAR2(100),
  SPOTSIZE      NVARCHAR2(100),
  CONCENT       NVARCHAR2(100),
  AMOUNT        NVARCHAR2(100),
  DATE_CRIME    TIMESTAMP(6) not null,
  CLASS_ID      NUMBER(10) not null,
  SORT_ID       NUMBER(10) not null,
  IDENT         NUMBER default 0 not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
comment on table MODERN.IK2
  is 'Карточка личности';
alter table MODERN.IK2
  add constraint PK_IK2 primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.IK2
  add constraint IK2_CARD_FK foreign key (ID)
  references MODERN.CARD (ID);
alter table MODERN.IK2
  add constraint IK2_CLASS_FK foreign key (CLASS_ID)
  references MODERN.CLASS_OBJECT (ID);
alter table MODERN.IK2
  add constraint IK2_CODELIN_FK foreign key (LIN_ID)
  references MODERN.CODE_LIN (ID);
alter table MODERN.IK2
  add constraint IK2_SORT_FK foreign key (SORT_ID)
  references MODERN.SORT_OBJECT (ID);
create index MODERN.ADDR_FX on MODERN.IK2 (ADDRESS_CRIME)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
create index MODERN.LIN_FX on MODERN.IK2 (LIN_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.MVD_FX on MODERN.IK2 (MVD_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table IK2_ARCH
prompt =======================
prompt
create table MODERN.IK2_ARCH
(
  ID            NUMBER(10) not null,
  MVD_ID        NUMBER(10) not null,
  LIN_ID        NUMBER(10) not null,
  YEAR_STATE    NUMBER not null,
  ADDRESS_CRIME NVARCHAR2(1024),
  VICTIM        NVARCHAR2(100),
  SN_DNA        NVARCHAR2(32),
  OBJ           NVARCHAR2(100),
  SPOTSIZE      NVARCHAR2(100),
  CONCENT       NVARCHAR2(100),
  AMOUNT        NVARCHAR2(100),
  DATE_CRIME    TIMESTAMP(6) not null,
  CLASS_ID      NUMBER(10) not null,
  SORT_ID       NUMBER(10) not null,
  IDENT         NUMBER default 0 not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.IK2_ARCH
  add constraint PK_IK2_ARCH primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.IK2_ARCH
  add constraint IK2_ARCH_CARD_FK foreign key (ID)
  references MODERN.CARD_ARCH (ID);
alter table MODERN.IK2_ARCH
  add constraint IK2_ARCH_CLASS_FK foreign key (CLASS_ID)
  references MODERN.CLASS_OBJECT (ID);
alter table MODERN.IK2_ARCH
  add constraint IK2_ARCH_LIN_FK foreign key (LIN_ID)
  references MODERN.CODE_LIN (ID);
alter table MODERN.IK2_ARCH
  add constraint IK2_ARCH_MVD_FK foreign key (MVD_ID)
  references MODERN.CODE_MVD (ID);
alter table MODERN.IK2_ARCH
  add constraint IK2_ARCH_SORT_OBJ_FK foreign key (SORT_ID)
  references MODERN.SORT_OBJECT (ID);
alter table MODERN.IK2_ARCH
  add constraint CKC_IDENT_IK2_ARCH
  check (IDENT between 0 and 1);
create index MODERN.ADDR_FX2 on MODERN.IK2_ARCH (ADDRESS_CRIME)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.LIN_FX2 on MODERN.IK2_ARCH (LIN_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
create index MODERN.MVD_FX2 on MODERN.IK2_ARCH (MVD_ID)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table ROLES_EXPERT
prompt ===========================
prompt
create table MODERN.ROLES_EXPERT
(
  ROLE_ID   NUMBER(10) not null,
  EXPERT_ID NUMBER(10) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ROLES_EXPERT
  add constraint PK_ROLES_EXPERT primary key (ROLE_ID, EXPERT_ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.ROLES_EXPERT
  add constraint RE_EXPERT_FK foreign key (EXPERT_ID)
  references MODERN.EXPERT (ID);
alter table MODERN.ROLES_EXPERT
  add constraint ROLE_EXP_FR foreign key (ROLE_ID)
  references MODERN.ROLES (ID);

prompt
prompt Creating table TEST
prompt ===================
prompt
create table MODERN.TEST
(
  ID    NUMBER not null,
  ARTCL VARCHAR2(512)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.TEST
  add primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table UKARTICL
prompt =======================
prompt
create table MODERN.UKARTICL
(
  ID    NUMBER(10) not null,
  ARTCL NVARCHAR2(10) not null,
  NOTE  NVARCHAR2(1024) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 128K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.UKARTICL
  add constraint PK_UKARTICL primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.UKARTICL
  add constraint FK_ARTICL_ITEM_FK foreign key (ID)
  references MODERN.UKITEM (ID) on delete cascade;
create index MODERN.ARTLC_IDX on MODERN.UKARTICL (ARTCL)
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table UKTEXT
prompt =====================
prompt
create table MODERN.UKTEXT
(
  ID   NUMBER(10) not null,
  NOTE NVARCHAR2(512) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.UKTEXT
  add constraint PK_UKTEXT primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
alter table MODERN.UKTEXT
  add constraint TEXT_ITEM_FK foreign key (ID)
  references MODERN.UKITEM (ID) on delete cascade;

prompt
prompt Creating sequence CARD_IDENT_SEQ
prompt ================================
prompt
create sequence MODERN.CARD_IDENT_SEQ
minvalue 128
maxvalue 999999999999999999999999999
start with 188
increment by 1
cache 20;

prompt
prompt Creating sequence CARD_SEQ
prompt ==========================
prompt
create sequence MODERN.CARD_SEQ
minvalue 3596
maxvalue 999999999999999999999999999
start with 4536
increment by 1
cache 20;

prompt
prompt Creating sequence CLASS_IKL_SEQ
prompt ===============================
prompt
create sequence MODERN.CLASS_IKL_SEQ
minvalue 52
maxvalue 999999999999999999999999999
start with 52
increment by 1
cache 20;

prompt
prompt Creating sequence CLASS_OBJECT_SEQ
prompt ==================================
prompt
create sequence MODERN.CLASS_OBJECT_SEQ
minvalue 51
maxvalue 999999999999999999999999999
start with 51
increment by 1
cache 20;

prompt
prompt Creating sequence CODE_LIN_SEQ
prompt ==============================
prompt
create sequence MODERN.CODE_LIN_SEQ
minvalue 48
maxvalue 999999999999999999999999999
start with 48
increment by 1
cache 20;

prompt
prompt Creating sequence CODE_MVD_SEQ
prompt ==============================
prompt
create sequence MODERN.CODE_MVD_SEQ
minvalue 97
maxvalue 999999999999999999999999999
start with 137
increment by 1
cache 20;

prompt
prompt Creating sequence DIVISION_SEQ
prompt ==============================
prompt
create sequence MODERN.DIVISION_SEQ
minvalue 10
maxvalue 999999999999999999999999999
start with 10
increment by 1
cache 20;

prompt
prompt Creating sequence EXPERT_SEQ
prompt ============================
prompt
create sequence MODERN.EXPERT_SEQ
minvalue 33
maxvalue 999999999999999999999999999
start with 73
increment by 1
cache 20;

prompt
prompt Creating sequence FILTERS_SEQ
prompt =============================
prompt
create sequence MODERN.FILTERS_SEQ
minvalue 1
maxvalue 999999999999999999999999999
start with 141
increment by 1
cache 20;

prompt
prompt Creating sequence HISTORY_SEQ
prompt =============================
prompt
create sequence MODERN.HISTORY_SEQ
minvalue 3006
maxvalue 999999999999999999999999999
start with 4766
increment by 1
cache 20;

prompt
prompt Creating sequence METHOD_SEQ
prompt ============================
prompt
create sequence MODERN.METHOD_SEQ
minvalue 2
maxvalue 999999999999999999999999999
start with 2
increment by 1
cache 20;

prompt
prompt Creating sequence ORGANIZATION_SEQ
prompt ==================================
prompt
create sequence MODERN.ORGANIZATION_SEQ
minvalue 783
maxvalue 999999999999999999999999999
start with 923
increment by 1
cache 20;

prompt
prompt Creating sequence POST_SEQ
prompt ==========================
prompt
create sequence MODERN.POST_SEQ
minvalue 29
maxvalue 999999999999999999999999999
start with 29
increment by 1
cache 20;

prompt
prompt Creating sequence ROLES_SEQ
prompt ===========================
prompt
create sequence MODERN.ROLES_SEQ
minvalue 1161
maxvalue 999999999999999999999999999
start with 1161
increment by 1
cache 20;

prompt
prompt Creating sequence SORT_OBJECT_SEQ
prompt =================================
prompt
create sequence MODERN.SORT_OBJECT_SEQ
minvalue 49
maxvalue 999999999999999999999999999
start with 49
increment by 1
cache 20;

prompt
prompt Creating sequence UKITEM_SEQ
prompt ============================
prompt
create sequence MODERN.UKITEM_SEQ
minvalue 516
maxvalue 999999999999999999999999999
start with 516
increment by 1
cache 20;

prompt
prompt Creating view V_CARD_IDENT
prompt ==========================
prompt
create or replace view modern.v_card_ident as
select "CARD_ID","ID", 0 arch from modern.card_ident
union all
select "CARD_ID","ID", 1 arch from modern.card_ident_arch;

prompt
prompt Creating view V_EXPERT
prompt ======================
prompt
create or replace view modern.v_expert as
select
  id,
  division_id,
  (select d.name||' '||d.address||' '||d.phone from modern.Division d where d.id = e.division_id) division,
  (select d.name from modern.division d where d.id = division_id) division_name,
   post_id,
  (select p.name from modern.post p where p.id = e.post_id) post,
   e.surname,
   e.name,
   e.patronic,
   login,
   e.sign
from modern.expert e;

prompt
prompt Creating view V_UKARTICL
prompt ========================
prompt
create or replace view modern.v_ukarticl as
select ua.id, to_char(ua.artcl) artcl  from modern.ukarticl ua;

prompt
prompt Creating view V_UKSTATE
prompt =======================
prompt
create or replace view modern.v_ukstate as
select u.id, t.artcl, t.note, u.parent_id from (
select id, artcl, note from modern.ukarticl
union all
select id, to_nchar('0'), note from modern.uktext
) t, modern.ukitem u
where u.id = t.id
and u.parent_id = 0;

prompt
prompt Creating type T_FIND_RESULT
prompt ===========================
prompt
CREATE OR REPLACE TYPE MODERN."T_FIND_RESULT"                                                                                 as object (
    "profile_id" number,
    "cnt" number,
    "card_type" number,
    "card_number" nvarchar2(100),
    "exam_num" nvarchar2(100)
)
/

prompt
prompt Creating type TBL_FIND_RESULT
prompt =============================
prompt
CREATE OR REPLACE TYPE MODERN."TBL_FIND_RESULT"                                                                                                                                                                 as table of T_FIND_RESULT
/

prompt
prompt Creating type T_PAIR_NUMBER
prompt ===========================
prompt
CREATE OR REPLACE TYPE MODERN."T_PAIR_NUMBER"                                                                                                                                                                 as object (
    "id_key" number,
    "id_value" number
)
/

prompt
prompt Creating type TBL_PAIR_NUMBER
prompt =============================
prompt
CREATE OR REPLACE TYPE MODERN."TBL_PAIR_NUMBER"                                                                                 as table of T_PAIR_NUMBER
/

prompt
prompt Creating package PKG_CARD
prompt =========================
prompt
create or replace package modern.PKG_CARD is

  -- Функции поиска
  function find(a_profile_id in number, a_allele_count in number, a_locus_count in number) return tbl_find_result pipelined;
  function find_half(a_profile_id in number, a_locus_count in number) return tbl_find_result pipelined;
  function find_half_one_parent(a_profile_child_id in number, a_profile_parent_id in number, a_locus_count in number) return tbl_find_result pipelined;
  function find_child_by_parents(a_profile_parent_one in number, a_profile_parent_second in number, a_locus_count in number) return tbl_find_result pipelined;
  function profile_by_child_and_parent(a_profile_child_id in number, a_profile_parent_id in number) return tbl_pair_number pipelined;


  -- Получение следующего номера история карточки
  function set_card_history(a_id in number, history_action in number, expert_id in number,
    note in nvarchar2) return number;

  -- Изменение номера истории карточки
  procedure updHistory(a_card_id in number, a_hist_id in number);

  -- Удаление карточки ИКЛ
  procedure ikl_del(a_id in number, curr_user in number);
  -- Создание карточки ИКЛ
  function ikl_ins(a_card_num in nvarchar2, a_class_id in number, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date date,
    a_exam_note in nvarchar2, a_date_ins in date, a_person in nvarchar2,
    a_ancillary in nvarchar2, curr_user in number) return number;
  -- Изменение карточки ИКЛ
  procedure ikl_upd (a_id in number, a_class_id in number, a_card_num in nvarchar2, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date date,
    a_exam_note in nvarchar2, a_date_ins in date, a_person in nvarchar2,
    a_ancillary in nvarchar2, curr_user in number);

  -- Удаление карточки ИК-2
  procedure ik2_del(a_id in number, curr_user in number);
  -- Создание карточки ИК-2
  function ik2_ins(a_card_num in nvarchar2, a_class_id in number, a_sort_id in number, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date in date,
    a_date_ins in date, a_mvd_id in number, a_lin_id in number,
    a_date_crim in date, a_victim in nvarchar2, a_sn_dna in nvarchar2, a_obj in nvarchar2,
    a_spotsize in nvarchar2, a_concent in nvarchar2, a_amount in nvarchar2, a_address_crim in nvarchar2,
    a_year_state in number, curr_user in number) return number;
  -- Обновление карточки ИК-2
  procedure ik2_upd(a_id in number, a_class_id in number, a_sort_id in number, a_card_num in nvarchar2, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date in date,
    a_date_ins in date, a_mvd_id in number, a_lin_id in number,
    a_date_crim in date, a_victim in nvarchar2, a_sn_dna in nvarchar2, a_obj in nvarchar2,
    a_spotsize in nvarchar2, a_concent in nvarchar2, a_amount in nvarchar2, a_address_crim in nvarchar2,
    a_year_state in number, curr_user in number);

  -- Обновление профиля
  procedure UpdataProfile(a_profile_id in number);

  -- Добавление аллели в профиль
  procedure AddAllele(a_profile_id in number, a_locus_id in number, a_allele_id in number);

  -- Получение названия статьи уголовного кодекса
  function UkState(a_id in number) return nvarchar2;

  -- Добавление уголовной статьи к карточке
  procedure uk_ins(a_card_id in number, a_uk_id in number);
  -- Удаление уголовный статей из карточки
  procedure uk_del(a_card_id in number);

  -- Идентификация карточки
  procedure ident(a_ik2 in number, a_card in number, a_curr_user in number);
  procedure un_ident(a_ik2 in number, a_card in number, a_curr_user in number);
  procedure un_ident_all(a_ik2 in number, a_curr_user in number);
  function card_kind(a_id in number) return number;

  -- Архив
  procedure remove_to_archive (a_id in number,curr_user in number,note in nclob);
  procedure extract_from_archive (a_id in number,curr_user in number,note in nclob);
end;
/

prompt
prompt Creating package PKG_COMMON
prompt ===========================
prompt
create or replace package modern.pkg_common is
  procedure set_sequencies(a_name in varchar2, a_value in number, a_inc in number);
  function max_id(a_name in varchar2) return number;
end;
/

prompt
prompt Creating package PKG_LOAD_METADATA
prompt ==================================
prompt
create or replace package modern.pkg_load_metadata is
  procedure load_data;
end;
/

prompt
prompt Creating type T_LIST_NUMBER
prompt ===========================
prompt
CREATE OR REPLACE TYPE MODERN."T_LIST_NUMBER"                                                                                 as object (
    "id" number,
    "note" nvarchar2(32)
)
/

prompt
prompt Creating type TBL_LIST_NUMBER
prompt =============================
prompt
CREATE OR REPLACE TYPE MODERN."TBL_LIST_NUMBER"                                                                                 as table of T_LIST_NUMBER
/

prompt
prompt Creating package PKG_SCR
prompt ========================
prompt
create or replace package modern.PKG_SCR is
  function Encrypt(a_str in nvarchar2, a_key in nvarchar2) return raw;
  function Decrypt(a_raw in raw, a_key in nvarchar2) return varchar2;
  function getHash(a_str in nvarchar2) return raw;
  function user_right(a_user_id in number) return tbl_list_number pipelined;
end;
/

prompt
prompt Creating package PRK_TAB
prompt ========================
prompt
create or replace package modern.prk_tab is

  -- Работа со справочниками
    -- Справочник "Коды МВД"
    function mvd_ins(a_name in nvarchar2, a_short_name in nvarchar2, a_code in nvarchar2) return number;
    procedure mvd_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2, a_code in nvarchar2);
    procedure mvd_del(a_id number);

    -- Справочник "Коды райлиноргана"
    function lin_ins(a_code in nvarchar2, a_organ in nvarchar2) return number;
    procedure lin_upd(a_id in number, a_code in nvarchar2, a_organ in nvarchar2);
    procedure lin_del(a_id number);

    -- Справочник "Список экспертных подразделений"
    function division_ins(a_name in nvarchar2, a_address in nvarchar2, a_phone in nvarchar2) return number;
    procedure division_upd(a_id in number, a_name in nvarchar2, a_address in nvarchar2, a_phone in nvarchar2);
    procedure division_del(a_id in number);

    -- Справочник "Список должностей"
    function post_ins(a_name in nvarchar2) return number;
    procedure post_upd(a_id in number, a_name in nvarchar2);
    procedure post_del(a_id in number);

    -- Справочник "Список экспертов"
    function exp_ins(a_div_id in number, a_post_id in number, a_surname in nvarchar2,
      a_name in nvarchar2, a_patronic in nvarchar2, a_login in nvarchar2, a_sign in nvarchar2) return number;
    procedure exp_upd(a_id in number, a_div_id in number, a_post_id in number, a_surname in nvarchar2,
      a_name in nvarchar2, a_patronic in nvarchar2, a_login in nvarchar2, a_sign in nvarchar2);
    procedure exp_del(a_id in number,  a_curr_user in number);
    procedure exp_change_password(a_id in number, a_password in nvarchar2);
    function exp_check_password(a_id in number, a_password in nvarchar2) return number;

    -- Справочник "Список методов расчета"
    function method_ins(a_name in nvarchar2, a_def_freq in number, a_desc in nvarchar2) return number;
    procedure freq_ins(a_allele_id in number, a_method_id in number, a_freq in number);
    procedure method_upd(a_id in number, a_name in nvarchar2, a_def_freq in number, a_desc in nvarchar2);
    procedure freq_upd(a_method_id in number, a_allele_id in number, a_freq in number);
    procedure method_del(a_id in number);
    procedure freq_del(a_method_id in number, a_allele_id in number);
    procedure methods_freq_del(a_id in number);

    -- Справочник "Статьи УК"
    function uk_artcl_ins(a_artcl in nvarchar2, a_note in nvarchar2, a_parent_id in number) return number;
    function uk_text_ins(a_note in nvarchar2, a_parent_id in number) return number;
    procedure uk_artcl_upd(a_id in number, a_artcl in nvarchar2, a_note in nvarchar2, a_parent_id in number);
    procedure uk_text_upd(a_id in number, a_note in nvarchar2, a_parent_id in number);
    procedure uk_item_del(a_id in number);

    -- Справочник "Роли пользователя"
    function role_ins(a_name in nvarchar2) return number;
    procedure role_upd(a_id in number, a_name in nvarchar2);
    procedure role_del(a_id in number);

    -- Справочник "Организации"
    function org_ins(a_note in nvarchar2) return number;
    procedure org_upd(a_id in number, a_note in nvarchar2);
    procedure org_del(a_id in number);

    -- Справочник "Вид объекта"
    function sort_ins(a_name in nvarchar2, a_short_name in nvarchar2) return number;
    procedure sort_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2);
    procedure sort_del(a_id in number);

    -- Справочник "Категории объекта"
    function class_obj_ins(a_name in nvarchar2, a_short_name in nvarchar2) return number;
    procedure class_obj_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2);
    procedure class_obj_del(a_id in number);

    -- Справочник "Категории карточки ИКЛ"
    function class_ikl_ins(a_name in nvarchar2, a_short_name in nvarchar2) return number;
    procedure class_ikl_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2);
    procedure class_ikl_del(a_id in number);

    -- Назначение прав ролям
    procedure role_right_del(a_role_id in number);
    procedure role_right_ins(a_role_id in number, a_func_id in number);

    -- Назначение прав пользователям
    procedure right_user_del(a_user_id in number);
    procedure right_user_ins(a_user_id in number, a_func_id in number, a_permission in number);
    procedure role_user_ins(a_user_id in number, a_role_id in number);

    -- Работа с фильтрами
    function filter_ins(a_name in nvarchar2, a_text in nclob) return number;
    procedure filter_upd(a_id in number, a_name in nvarchar2, a_text in nvarchar2);
    procedure filter_del(a_id in number);

end ;
/

prompt
prompt Creating type T_STRING_AGG
prompt ==========================
prompt
CREATE OR REPLACE TYPE MODERN.t_string_agg AS OBJECT
(
  g_string  VARCHAR2(32767),
  STATIC FUNCTION ODCIAggregateInitialize(sctx IN OUT t_string_agg) RETURN NUMBER,
  MEMBER FUNCTION ODCIAggregateIterate(self IN OUT t_string_agg, value IN VARCHAR2 ) RETURN NUMBER,
  MEMBER FUNCTION ODCIAggregateTerminate(self IN t_string_agg, returnValue OUT VARCHAR2, flags IN NUMBER) RETURN NUMBER,
  MEMBER FUNCTION ODCIAggregateMerge(self IN OUT t_string_agg,ctx2 IN t_string_agg) RETURN NUMBER
);
/

prompt
prompt Creating type T_STRING_AGG_SPACE
prompt ================================
prompt
CREATE OR REPLACE TYPE MODERN.t_string_agg_space AS OBJECT
(
  g_string  VARCHAR2(32767),
  STATIC FUNCTION ODCIAggregateInitialize(sctx IN OUT t_string_agg_space) RETURN NUMBER,
  MEMBER FUNCTION ODCIAggregateIterate(self IN OUT t_string_agg_space, value IN VARCHAR2 ) RETURN NUMBER,
  MEMBER FUNCTION ODCIAggregateTerminate(self IN t_string_agg_space, returnValue OUT VARCHAR2, flags IN NUMBER) RETURN NUMBER,
  MEMBER FUNCTION ODCIAggregateMerge(self IN OUT t_string_agg_space, ctx2 IN t_string_agg_space) RETURN NUMBER
);
/

prompt
prompt Creating function STRING_AGG_SPACE
prompt ==================================
prompt
CREATE OR REPLACE FUNCTION MODERN.string_agg_space (p_input VARCHAR2) RETURN VARCHAR2
PARALLEL_ENABLE AGGREGATE USING t_string_agg_space;
/

prompt
prompt Creating function GET_STATE
prompt ===========================
prompt
create or replace function modern.get_state(ukitem_id in number) return nvarchar2 as
  artcl nvarchar2(32767);
begin
select string_agg_space(to_char(artcl)) into artcl from (
select
 case
   when num = 1 then 'ст.'||artcl
   when num = 2 then 'ч.'||artcl
   when num = 3 then 'п.'||artcl
   end artcl
 from (
select row_number() over(order by id) num, t.artcl from (
select u.id, ua.artcl, level lvl from modern.ukitem u, modern.ukarticl ua
where u.id = ua.id
connect by prior u.parent_id = u.id
start with u.id = ukitem_id
order siblings by u.parent_id
) t
where id <> 0
order by lvl desc
));
return artcl;
end;
/

prompt
prompt Creating function LOCUS_CNT
prompt ===========================
prompt
create or replace function modern.locus_cnt(a_profile_1 in number,
                                            a_profile_2 in number)
  return number is
  cnt number default 0;
begin
  select count(*) into cnt from (
  select locus_id
    from modern.chk_allele ca
   where ca.profile_id = a_profile_1
     and ca.locus_id in
         (select locus_id from modern.chk_allele where profile_id = a_profile_2)
   group by locus_id);
   return cnt;
end;
/

prompt
prompt Creating function STRING_AGG
prompt ============================
prompt
CREATE OR REPLACE FUNCTION MODERN.string_agg (p_input VARCHAR2) RETURN VARCHAR2
PARALLEL_ENABLE AGGREGATE USING t_string_agg;
/

prompt
prompt Creating procedure COMPARE_DICTIONARY
prompt =====================================
prompt
create or replace procedure modern.COMPARE_DICTIONARY(a_old_id     in number,
                                                      a_new_id     in number,
                                                      a_table_name in varchar2,
																											a_primary_key in varchar2,
																											a_owner in varchar2) is
begin
  for c in (select c1.owner,
                   c1.TABLE_NAME,
                   (select column_name
                      from sys.all_cons_columns
                     where owner = c1.OWNER
                       and table_name = c1.TABLE_NAME
                       and constraint_name = c1.CONSTRAINT_NAME) cln
              from sys.all_constraints c1
             where r_owner = UPPER(a_owner)
               and r_constraint_name =
                   (select constraint_name
                      from sys.DBA_CONSTRAINTS
                     where owner = UPPER(a_owner)
                       and table_name = UPPER(a_table_name)
                       and constraint_type = 'P'))
  loop
		-- Изменяем значения столбцов в связанных таблицах
		EXECUTE IMMEDIATE 'update '||c.owner||'.'||c.table_name||
		' set '||c.cln||' = '||a_new_id||' where '||c.cln||' = '||a_old_id;
  end loop;
		-- Удаляем запись из справочника
		EXECUTE IMMEDIATE 'delete from '||UPPER(a_owner)||'.'||UPPER(a_table_name)||
		' where '||a_primary_key||' ='||a_old_id;
end;
/

prompt
prompt Creating procedure LOGON
prompt ========================
prompt
create or replace procedure modern.logon(
  a_name in nvarchar2,
  a_pass in nvarchar2,
  a_name_out out nvarchar2,
  a_pass_out out nvarchar2,
  a_user_name_out out nvarchar2,
  a_user_login_out out nvarchar2,
  a_user_id_out out number) is
begin
  begin
    select id, name, login into a_user_id_out, a_user_name_out, a_user_login_out from modern.expert e
      where upper(trim(e.login)) = upper(a_name) and e.password = modern.PKG_SCR.getHash(a_pass||salt);
      a_name_out := 'system';
      a_pass_out := 'sys';
  exception
    when others then
      a_name_out := '';
      a_pass_out := '';
      a_user_id_out := 0;
      a_user_name_out := '';
      a_user_login_out := '';
  end;
end;
/

prompt
prompt Creating package body PKG_CARD
prompt ==============================
prompt
create or replace package body modern.PKG_CARD is
  -- Получаем историю карточки
  function set_card_history(a_id in number, history_action in number, expert_id in number,
    note in nvarchar2) return number is
      hist_id number;
      new_hist_id number;
  begin
    if (a_id is not null) and (a_id <> 0) then
      select c.history_id into hist_id from modern.card c where c.id = a_id;
    end if;

    select modern.history_seq.nextval into new_hist_id from dual;
    if (a_id is not null) and (a_id <> 0) then
      insert into modern.history(id, expert_id, action_id, DATE_INSERT, note, parent_id) values
        (new_hist_id, expert_id, history_action, sysdate, note, hist_id);
      update modern.card c set c.history_id = new_hist_id where c.id = a_id;
    else
      insert into modern.history(id, expert_id, action_id, DATE_INSERT, note) values
        (new_hist_id, expert_id, history_action, sysdate, note);
    end if;

    return new_hist_id;
  end;

  -- Изменение номера истории карточки
  procedure updHistory(a_card_id in number, a_hist_id in number) is
  begin
    update modern.card c set c.history_id = a_hist_id where c.id = a_card_id;
  end;

  -- Карточка ИКЛ
  procedure ikl_del(a_id in number, curr_user in number) is
    hist_id number;
  begin
    hist_id := set_card_history(a_id, 5, curr_user, 'Удаление карточки ИКЛ ID='||a_id);
    delete from modern.chk_allele ca where ca.profile_id = a_id;
    delete from modern.profile p where p.id = a_id;
    delete from modern.ikl where id = a_id;
    delete from modern.card_ukitem where card_id = a_id;
    delete from modern.correct_uk where id = a_id;
    delete from modern.card where id = a_id;
  end;

  function ikl_ins(a_card_num in nvarchar2, a_class_id in number, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date date,
    a_exam_note in nvarchar2, a_date_ins in date, a_person in nvarchar2,
    a_ancillary in nvarchar2, curr_user in number) return number is
    card_id  number;
    hist_id number;
  begin
    select modern.card_seq.nextval into card_id from dual;

    -- Заводим историю карточки
    hist_id := set_card_history(null, 1, curr_user,
       'Создание карточки ИКЛ ID="'||card_id||'" Номер="'||a_card_num||'"');

    -- Сохраняем данные по карточке
    insert into modern.card(id, card_num, crim_num,org_id, expert_id, exam_num, exam_date,
      exam_note, date_ins, history_id, hash) values
      (card_id, a_card_num, a_crim_num, a_org_id, a_expert_id, a_exam_num, a_exam_date,
      a_exam_note, a_date_ins, hist_id, upper(card_id||a_card_num||a_crim_num||a_person||a_date_ins));
    insert into modern.ikl(id, person, ancillary, class_id) values (card_id, a_person, a_ancillary, a_class_id);

    return card_id;
  end;

  procedure ikl_upd (a_id in number, a_class_id in number, a_card_num in nvarchar2, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date date,
    a_exam_note in nvarchar2, a_date_ins in date, a_person in nvarchar2,
    a_ancillary in nvarchar2, curr_user in number) is
    hist_id number;
  begin
    hist_id := set_card_history(a_id, 3, curr_user,
      'Изменение данных для карточки ИКЛ ID="'||a_id||'" Номер="'||a_card_num||'"  '||
      'Новые данные: a_card_num='||a_card_num||' a_class_id='||a_class_id||' a_crim_num='||a_crim_num||' a_org_id='||a_org_id||' a_expert_id='||a_expert_id||
      ' a_exam_num='||a_exam_num||' a_exam_date='||a_exam_date||' a_exam_note='||a_exam_note||
      ' a_date_ins='||a_date_ins||' a_person='||a_person||' a_ancillary='||a_ancillary);

    update modern.card c
      set
        c.card_num = a_card_num, c.crim_num = a_crim_num,
        c.org_id = a_org_id, c.expert_id = a_expert_id, c.exam_num = a_exam_num,
        c.exam_date = a_exam_date, c.exam_note = a_exam_note, c.date_ins = a_date_ins,
        c.history_id = hist_id,
        c.hash = upper(a_id||a_card_num||a_crim_num||a_person||a_date_ins)
      where
        c.id = a_id;

    update modern.ikl i
      set
        i.person = a_person,
        i.ancillary = a_ancillary,
        i.class_id = a_class_id
      where
        i.id = a_id;
  end;

  function ik2_ins(a_card_num in nvarchar2, a_class_id in number, a_sort_id in number, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date in date,
    a_date_ins in date, a_mvd_id in number, a_lin_id in number,
    a_date_crim in date, a_victim in nvarchar2, a_sn_dna in nvarchar2, a_obj in nvarchar2,
    a_spotsize in nvarchar2, a_concent in nvarchar2, a_amount in nvarchar2, a_address_crim in nvarchar2,
    a_year_state in number, curr_user in number) return number is
      card_id  number;
      hist_id number;
    begin
      select modern.card_seq.nextval into card_id from dual;

      -- Заводим историю карточки
      hist_id := set_card_history(null, 1, curr_user,
         'Создание карточки ИК-2 ID="'||card_id||'" Номер="'||a_card_num||'"');

      -- Сохраняем данные по карточке
      insert into modern.card(id, card_num, crim_num, org_id, expert_id, exam_num, exam_date,
        exam_note, date_ins, history_id, hash) values
        (card_id, a_card_num, a_crim_num, a_org_id, a_expert_id, a_exam_num, a_exam_date,
        '', a_date_ins, hist_id, upper(card_id||a_card_num||a_crim_num||a_victim||a_date_ins));
      insert into modern.ik2(id, class_id, sort_id, mvd_id, lin_id, year_state, address_crime, victim, sn_dna,
        obj, spotsize, concent, amount, date_crime) values (card_id, a_class_id, a_sort_id, a_mvd_id, a_lin_id, a_year_state,
        a_address_crim, a_victim, a_sn_dna, a_obj, a_spotsize, a_concent, a_amount, a_date_crim);
      return card_id;
    end;

    -- Удаление карточки ИК-2
  procedure ik2_del(a_id in number, curr_user in number) is
    hist_id number;
  begin
    hist_id := set_card_history(a_id, 5, curr_user, 'Удаление карточки ИК-2 ID='||a_id);
    delete from modern.chk_allele ca where ca.profile_id = a_id;
    delete from modern.profile p where p.id = a_id;
    delete from modern.ik2 i where i.id = a_id;
    delete from modern.card_ukitem where card_id = a_id;
    delete from modern.correct_uk where id = a_id;
    delete from modern.card c where c.id = a_id;
  end;

  procedure UpdataProfile(a_profile_id in number) is
    cnt number;
  begin
    select count(*) into cnt from modern.profile p where p.id = a_profile_id;
    if (cnt = 0) then
      insert into modern.profile(id) values (a_profile_id);
    else
      delete from modern.chk_allele ca where ca.profile_id = a_profile_id;
    end if;
  end;

  procedure ik2_upd(a_id in number, a_class_id in number, a_sort_id in number, a_card_num in nvarchar2, a_crim_num in nvarchar2,
    a_org_id in number, a_expert_id in number, a_exam_num in nvarchar2, a_exam_date in date,
    a_date_ins in date, a_mvd_id in number, a_lin_id in number,
    a_date_crim in date, a_victim in nvarchar2, a_sn_dna in nvarchar2, a_obj in nvarchar2,
    a_spotsize in nvarchar2, a_concent in nvarchar2, a_amount in nvarchar2, a_address_crim in nvarchar2,
    a_year_state in number, curr_user in number) is
      card_id  number;
      hist_id number;
    begin
    hist_id := set_card_history(a_id, 4, curr_user,
      'Изменение данных для карточки ИК-2 ID="'||a_id||'" Номер="'||a_card_num||'"  '||
      'Новые данные: a_card_num='||a_card_num||' a_crim_num='||a_crim_num||' a_org_id='||a_org_id||' a_expert_id='||a_expert_id||
      ' a_exam_num='||a_exam_num||' a_date_ins='||a_date_ins||' a_mvd_id='||a_mvd_id||
      ' a_lin_id='||a_lin_id||' a_date_crim='||a_date_crim||' a_victim='||a_victim||' a_sn_dna='||a_sn_dna||
      ' a_obj='||a_obj||' a_spotsize='||a_spotsize||' a_concent='||a_concent||' a_amount='||a_amount||
      ' a_address_crim='||a_address_crim||'a_year_state='||a_year_state
      );

    update modern.card c
      set
        c.card_num = a_card_num,
        c.crim_num = a_crim_num,
        c.org_id = a_org_id,
        c.expert_id = a_expert_id,
        c.exam_num = a_exam_num,
        c.exam_date = a_exam_date,
        c.date_ins = a_date_ins,
        c.history_id = hist_id,
        c.hash = upper(a_id||a_card_num||a_crim_num||a_victim||a_date_ins)
      where
        c.id = a_id;

    update modern.ik2 i
      set
        i.mvd_id = a_mvd_id,
        i.lin_id = a_lin_id,
        i.year_state = a_year_state,
        i.address_crime = a_address_crim,
        i.victim = a_victim,
        i.sn_dna= a_sn_dna,
        i.obj = a_obj,
        i.spotsize = a_spotsize,
        i.concent = a_concent,
        i.date_crime = a_date_crim,
        i.amount = a_amount,
        i.class_id = a_class_id,
        i.sort_id = a_sort_id
      where
        i.id = a_id;
  end;


  procedure AddAllele(a_profile_id in number, a_locus_id in number, a_allele_id in number) is
  begin
    insert into modern.chk_allele(profile_id, locus_id,allele_id)
      values(a_profile_id, a_locus_id, a_allele_id);
  end;

  function UkState(a_id in number) return nvarchar2 as
    result nvarchar2(512);
  begin
    for c in (
      select artcl, note from (
      select id, artcl, note, (select parent_id from modern.ukitem where id = ua.id) parent_id from modern.ukarticl ua
      union all
      select id, to_nchar('0'), note, (select parent_id from modern.ukitem where id = ut.id) parent_id  from modern.uktext ut)
      connect by prior parent_id = id start with  id = a_id order by parent_id
    )
    loop
      if (c.artcl = '0') then
        return c.note;
      else
        if (result is not null) then
          result := result||';'||c.artcl;
        else
          result := c.artcl;
        end if;
      end if;
    end loop;
    return result;
  end;

  procedure uk_ins(a_card_id in number, a_uk_id in number) as
  begin
    insert into modern.card_ukitem(card_id, ukitem_id) values (a_card_id, a_uk_id);
  end;

  procedure uk_del(a_card_id in number) as
  begin
    delete from modern.card_ukitem where card_id = a_card_id;
  end;

  function find(a_profile_id in number, a_allele_count in number, a_locus_count in number) return tbl_find_result pipelined is
    cnt number default 0;
  begin
    select count(*) into cnt from modern.ikl ikl where ikl.id = a_profile_id;

    if (cnt = 0) then -- карта ик-2
      for rec in (
        select prf as profile_id, count(*)-sum(chk) cnt,
               case when (select count(*) from modern.ikl ikl where ikl.id = prf) = 0
                then 1
                else 0
               end card_type,
               (select card_num from modern.card where id = prf) card_num,
               (select exam_num from modern.card where id = prf) exam_num
          from (select a.profile_id as prf,
                       locus_id,
                       case
                         when a.allele_id in
                              (select allele_id
                                 from modern.chk_allele
                                where profile_id = a_profile_id) then
                          1
                         else
                          0
                       end chk
                  from modern.chk_allele a
                 where a.locus_id in (select locus_id
                                        from modern.chk_allele
                                       where profile_id = a_profile_id) and a.profile_id <> a_profile_id) t
         group by prf
        having (count(*)-sum(chk) < a_allele_count) and modern.locus_cnt(a_profile_id, prf) > a_locus_count
      ) loop
        pipe row(t_find_result(rec.profile_id, rec.cnt, rec.card_type, rec.card_num, rec.exam_num));
      end loop;
    else -- карта ИКЛ
      for rec in (
        select prf as profile_id, c-sum(chk) cnt,
               case
                 when (select count(*) from modern.ikl ikl where ikl.id = prf) = 0
                   then 1
                   else 0
                 end card_type,
               (select card_num from modern.card where id = prf) card_num,
               (select exam_num from modern.card where id = prf) exam_num
          from (select a.profile_id as prf,
                       locus_id,
                       (select count(*)
                          from modern.chk_allele
                         where profile_id = a_profile_id
                           and locus_id in
                               (select locus_id
                                  from modern.chk_allele
                                 where profile_id = a.profile_id)) c,
                       case
                         when a.allele_id in
                              (select allele_id
                                 from modern.chk_allele
                                where profile_id = a_profile_id) then
                          1
                         else
                          0
                       end chk
                  from modern.chk_allele a
                 where a.locus_id in (select locus_id
                                        from modern.chk_allele
                                       where profile_id = a_profile_id) and a.profile_id <> a_profile_id) t
         group by prf
        having (c - sum(chk) < a_allele_count) and modern.locus_cnt(a_profile_id, prf) > a_locus_count
      ) loop
        pipe row(t_find_result(rec.profile_id, rec.cnt, rec.card_type, rec.card_num, rec.exam_num));
      end loop;
    end if;

  end;

  function find_half(a_profile_id in number, a_locus_count in number) return tbl_find_result pipelined is
  begin
    for rec in (
      select prf as profile_id, count(locus_id) - 1 cnt,
               case
                 when (select count(*) from modern.ikl ikl where ikl.id = prf) = 0
                   then 1
                   else 0
                 end card_type,
               (select card_num from modern.card where id = prf) card_num,
               (select exam_num from modern.card where id = prf) exam_num
       from (
        select a.profile_id as prf, locus_id, round(avg(case
           when a.allele_id in (select allele_id from modern.chk_allele where profile_id = a_profile_id) then
           1 else 0 end)) chk
      from modern.chk_allele a
      where a.locus_id in (select locus_id from modern.chk_allele where profile_id = a_profile_id)
      and a.profile_id <> a_profile_id group by a.profile_id, locus_id) group by prf
      having count(locus_id) = sum(chk) and count(locus_id) > a_locus_count
    ) loop
      pipe row(t_find_result(rec.profile_id, rec.cnt, rec.card_type, rec.card_num, rec.exam_num));
    end loop;
  end;

  function find_half_one_parent(a_profile_child_id in number, a_profile_parent_id in number, a_locus_count in number) return tbl_find_result pipelined is
  begin
    for rec in (
      select prf as profile_id, count(locus_id)-1 cnt,
               case
                 when (select count(*) from modern.ikl ikl where ikl.id = prf) = 0
                   then 1
                   else 0
                 end card_type,
               (select card_num from modern.card where id = prf) card_num,
               (select exam_num from modern.card where id = prf) exam_num
       from (
        select a.profile_id as prf, locus_id, round(avg(case
           when a.allele_id in (

        select allele_id from modern.chk_allele where profile_id = a_profile_child_id
        and locus_id in (
          select locus_id from modern.chk_allele where profile_id = a_profile_child_id
            and locus_id not in (select locus_id from modern.chk_allele where profile_id = a_profile_child_id
            and allele_id not in (select allele_id from modern.chk_allele where profile_id = a_profile_parent_id)
            ))
          union all
          select allele_id from modern.chk_allele where profile_id = a_profile_child_id
           and allele_id not in (select allele_id from modern.chk_allele where profile_id = a_profile_parent_id)

             ) then
           1 else 0 end)) chk
      from modern.chk_allele a
      where a.locus_id in (

        select locus_id from modern.chk_allele where profile_id = a_profile_child_id
        and locus_id in (
          select locus_id from modern.chk_allele where profile_id = a_profile_child_id
            and locus_id not in (select locus_id from modern.chk_allele where profile_id = a_profile_child_id
            and allele_id not in (select allele_id from modern.chk_allele where profile_id = a_profile_parent_id)
            ))
          union all
          select locus_id from modern.chk_allele where profile_id = a_profile_child_id
           and allele_id not in (select allele_id from modern.chk_allele where profile_id = a_profile_parent_id)
      )
      and a.profile_id <> a_profile_child_id group by a.profile_id, locus_id) group by prf
      having count(locus_id) = sum(chk) and count(locus_id) > a_locus_count
    ) loop
      pipe row(t_find_result(rec.profile_id, rec.cnt, rec.card_type, rec.card_num, rec.exam_num));
    end loop;
  end;

  function find_child_by_parents(a_profile_parent_one in number, a_profile_parent_second in number, a_locus_count in number) return tbl_find_result pipelined is
  begin
    for rec in (
        select profile_id,
               cnt,
               case
                 when (select count(*) from modern.ikl ikl where ikl.id = profile_id) = 0 then
                  1
                 else
                  0
               end card_type,
               (select card_num from modern.card where id = profile_id) card_num,
               (select exam_num from modern.card where id = profile_id) exam_num
          from (
            select profile_id, count(locus_id) cnt from
            (select profile_id, locus_id, max(profile1) * max(profile2) * min(mult) cnt
              from (select t.*, greatest(t.profile1, t.profile2) mult
                      from (select a.profile_id,
                                   a.locus_id,
                                   case
                                     when a.allele_id in
                                          (select allele_id
                                             from modern.chk_allele
                                            where profile_id = a_profile_parent_one) then
                                      1
                                     else
                                      0
                                   end profile1,
                                   case
                                     when a.allele_id in
                                          (select allele_id
                                             from modern.chk_allele
                                            where profile_id = a_profile_parent_second) then
                                      1
                                     else
                                      0
                                   end profile2
                              from modern.chk_allele a
                             where a.locus_id in (select locus_id
                                                    from modern.chk_allele
                                                   where profile_id = a_profile_parent_one
                                                  intersect
                                                  select locus_id
                                                    from modern.chk_allele
                                                   where profile_id = a_profile_parent_second)
                               and a.profile_id not in (a_profile_parent_one, a_profile_parent_second)) t) tt
             group by profile_id, locus_id
             )
             group by profile_id
             having count(locus_id) = sum(cnt) and count(locus_id) > a_locus_count)

    ) loop
      pipe row(t_find_result(rec.profile_id, rec.cnt, rec.card_type, rec.card_num, rec.exam_num));
    end loop;
  end;

  function profile_by_child_and_parent(a_profile_child_id in number, a_profile_parent_id in number) return tbl_pair_number pipelined is
  begin
    for rec in (
      select allele_id, locus_id from modern.chk_allele where profile_id = a_profile_child_id
        and locus_id in (
          select locus_id from modern.chk_allele where profile_id = a_profile_child_id
            and locus_id not in (select locus_id from modern.chk_allele where profile_id = a_profile_child_id
            and allele_id not in (select allele_id from modern.chk_allele where profile_id = a_profile_parent_id)
            ))
          union all
          select allele_id, locus_id from modern.chk_allele where profile_id = a_profile_child_id
           and allele_id not in (select allele_id from modern.chk_allele where profile_id = a_profile_parent_id)
    ) loop
      pipe row(t_pair_number(rec.allele_id, rec.locus_id));
    end loop;
  end;

  -- Идентификация карточки
  procedure ident(a_ik2 in number, a_card in number, a_curr_user in number) is
    hist_id number;
    cnt number;
    p_group_id number;
  begin
    hist_id := set_card_history(a_ik2, 11, a_curr_user, 'Карточка ID='||a_ik2||' идентифицирована с карточкой'||a_card);
    begin
      select id into p_group_id from modern.v_card_ident where card_id = a_ik2;
    exception
      when no_data_found then
        select modern.card_ident_seq.nextval into p_group_id from dual;
        insert into modern.card_ident(card_id, id) values (a_ik2, p_group_id);
    end;
    insert into modern.card_ident(card_id, id) values (a_card, p_group_id);

    select count(*) into cnt from modern.ikl where id = a_card;
    if (cnt > 0) then
      update modern.ik2 i set i.ident = 1 where i.id in
      (select card_id from modern.v_card_ident ci, modern.ik2 i2
      where ci.card_id = i2.id and ci.id = p_group_id);
    end if;

  end;
  procedure un_ident(a_ik2 in number, a_card in number, a_curr_user in number) is
    hist_id number;
    cnt number;
    p_group_id number;
  begin
    hist_id := set_card_history(a_ik2, 12, a_curr_user, 'Идентификация для карточки ID='||a_ik2||' с карточкой'||a_card||' отменена.');
    select id into p_group_id from modern.v_card_ident where card_id = a_ik2;

    select count(*) into cnt from modern.ikl where id = a_card;
    if (cnt = 0) then
      update modern.ik2 i set i.ident = 0 where i.id = a_card;
    end if;

    delete from modern.card_ident where card_id = a_card and id = p_group_id;

    select count(*) into cnt from modern.ikl where id in (select card_id from
      modern.v_card_ident where id = p_group_id);
    if (cnt = 0) then
      update modern.ik2 i set i.ident = 0 where i.id in
      (select card_id from modern.v_card_ident ci, modern.ik2 i2
      where ci.card_id = i2.id and ci.id = p_group_id);
    end if;
    select count(*) into cnt from modern.v_card_ident where id = p_group_id;
    if (cnt = 1) then -- Удаляется последняя запись, потому, что одна запись не может коррелировать сама с собой
      delete from modern.card_ident where id = p_group_id;
    end if;
  end;
  function card_kind(a_id in number) return number is
    cnt number;
  begin
    select  nvl(i.id, -1) into cnt from modern.card c 
           left join modern.ikl i on c.id = i.id where c.id = a_id;
    if (cnt = -1) then
      return 1;
    else
      return 0;
    end if;    
  exception
    when no_data_found then
      return -1;           
  end;
  procedure un_ident_all(a_ik2 in number, a_curr_user in number) is
    hist_id number;
    ids varchar2(512);
  begin
    hist_id := set_card_history(a_ik2, 12, a_curr_user, 'Идентификация для карточки ID='||a_ik2||' с карточками: '||ids||' отменена.');
    delete from modern.card_ident where id = (select id from
      modern.card_ident where card_id = a_ik2);
    update modern.ik2 i set i.ident = 0 where i.id = a_ik2;
  end;
  -- Архив
  procedure remove_to_archive (a_id in number,curr_user in number,note in nclob) is
    hist_id number;
    cnt number default 0;
  begin
    hist_id := set_card_history(a_id, 9, curr_user, note);
    -- добавление в архив
    insert into modern.card_arch select * from modern.card where id = a_id;
    insert into modern.card_ident_arch select * from modern.card_ident where card_id = a_id;
    insert into modern.card_ukitem_arch select * from modern.card_ukitem where card_id = a_id;
    insert into modern.profile_arch select * from modern.profile where id = a_id;
    insert into modern.chk_allele_arch select * from modern.chk_allele where profile_id = a_id;
    insert into modern.ikl_arch select * from modern.ikl where id = a_id;
    insert into modern.ik2_arch select * from modern.ik2 where id = a_id;

    -- удаление записей из базы
    delete from modern.ik2 where id = a_id;
    delete from modern.ikl where id = a_id;
    delete from modern.chk_allele where profile_id = a_id;
    delete from modern.profile where id = a_id;
    delete from modern.correct_uk where id = a_id;
    delete from modern.card_ukitem where card_id = a_id;
    delete from modern.card_ident where card_id = a_id;
    delete from modern.card where id = a_id;
  end;
  procedure extract_from_archive (a_id in number,curr_user in number, note in nclob) is
    hist_id number;
    cnt number default 0;
  begin
    -- добавление в архив
    insert into modern.card select * from modern.card_arch where id = a_id;
    insert into modern.card_ident select * from modern.card_ident_arch where card_id = a_id;
    insert into modern.card_ukitem select * from modern.card_ukitem_arch where card_id = a_id;
    insert into modern.profile select * from modern.profile_arch where id = a_id;
    insert into modern.chk_allele select * from modern.chk_allele_arch where profile_id = a_id;
    insert into modern.ikl select * from modern.ikl_arch where id = a_id;
    insert into modern.ik2 select * from modern.ik2_arch where id = a_id;

    hist_id := set_card_history(a_id, 10, curr_user, note);

    -- удаление записей из базы
    delete from modern.ik2_arch where id = a_id;
    delete from modern.ikl_arch where id = a_id;
    delete from modern.chk_allele_arch where profile_id = a_id;
    delete from modern.profile_arch where id = a_id;
    delete from modern.card_ukitem_arch where card_id = a_id;
    delete from modern.card_ident_arch where card_id = a_id;
    delete from modern.card_arch where id = a_id;

  end;

end;
/

prompt
prompt Creating package body PKG_COMMON
prompt ================================
prompt
create or replace package body modern.pkg_common is
  procedure set_sequencies(a_name in varchar2, a_value in number, a_inc in number) is
  begin
    begin
      EXECUTE IMMEDIATE 'drop sequence '||a_name;
    exception
      when others then null;
    end;
    EXECUTE IMMEDIATE 'create sequence '||a_name||' minvalue '||a_value||' maxvalue 999999999999999999999999999 start with '||a_value||' increment by '||a_inc||' cache 20';
  end;

  function max_id(a_name in varchar2) return number is
    cnt number;
    sql_str varchar2(256);
  begin
    sql_str := 'select max(id) from '||a_name;
    EXECUTE IMMEDIATE sql_str into cnt;
    if (cnt is null) then
      return 0;
    end if;
    return cnt;
  end;

end;
/

prompt
prompt Creating package body PKG_LOAD_METADATA
prompt =======================================
prompt
create or replace package body modern.pkg_load_metadata is
  procedure load_data is
    salt varchar2(4);
    curr_state_val number;
    curr_past_val number;
  begin
    -- Типы событий записвываемых в историю
    insert into modern.history_action (ID, NAME,  NOTE) values (-1000, 'Added new value', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (1, 'Создание новой карточки ИКЛ', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (2, 'Создание новой карточки ИК-2', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (3, 'Изменение данных карточки ИКЛ', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (4, 'Изменение данных карточки ИК-2', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (5, 'Удаление карточки ИКЛ', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (6, 'Удаление карточки ИК-2', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (7, 'Редактирование профиля карточки', '');
    insert into modern.history_action (ID, NAME,  NOTE) values (8, 'Удаление пользователя', '');
    insert into modern.history_action(id, NAME, NOTE) values (9, 'Перемещение карточки в архив', '');
    insert into modern.history_action(id, NAME, NOTE) values (10, 'Восстановление карточки зи архива', '');
    insert into modern.history_action(id, NAME, NOTE) values (11, 'Идентификация карточки', '');
    insert into modern.history_action(id, NAME, NOTE) values (12, 'Отмена идентификации карточки', '');

    -- Список методов для которых задаются права доступа
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (1, 'none', 'Отсутствие функционала, используется для назначения тем ресурсам, которые не несут функциональной нагрузки, но используются в интерфейсе пользователя, например: для группировки связанных функций', 'none');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (2, 'CardFindShow', 'Форма поиска карточек', 'Найти карточку');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (3, 'CardIklNew', 'Форма создания новой карточки ИКЛ', 'Создать ИКЛ');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (4, 'CardIk2New', 'Форма создания новой карточки ИК-2', 'Создать ИК-2');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (5, 'CodeMvdList', 'Список кодов МВД', 'Коды МВД');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (6, 'CodeLinList', 'Список кодов райлинорганов', 'Коды райлинорганов');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (7, 'DivisionList', 'Список кодов подразделений', 'Список подразделений');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (8, 'PostList', 'Список должностей', 'Должности');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (9, 'ExpertList', 'Список экспертов', 'Эксперты');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (10, 'MethList', 'Методы расчета', 'Методы расчета');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (11, 'UkLIst', 'Список статей уголовного кодекса', 'Статьи УК');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (12, 'AdminShow', 'Форма назначения прав пользователям', 'Назначение прав');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (13, 'RoleList', 'Список ролей пользователй в системе', 'Роли');
    insert into modern.protect_function (ID, NAME, NOTE, CAPTION) values (14, 'OrganizationList', 'Список организаций', 'Организации');
    insert into modern.protect_function(id, name, note, caption) values (15, 'Карточки', 'Карточки', 'Карточки');
    insert into modern.protect_function(id, name, note, caption) values (16, 'Администрирование', 'Администрирование', 'Администрирование');
    insert into modern.protect_function(id, name, note, caption) values (17, 'Открытые формы', 'Открытые формы', 'Открытые формы');
    insert into modern.protect_function(id, name, note, caption) values (18, 'Справочники', 'Справочники', 'Справочники');
    insert into modern.protect_function(id, name, note, caption) values (19, 'Виды объекта', 'Список видов объекта', 'Виды объекта');
    insert into modern.protect_function(id, name, note, caption) values (20, 'Категории объекта', 'Список категорий объекта', 'Категории объекта');
    insert into modern.protect_function(id, name, note, caption) values (21, 'Категории ИКЛ', 'Список категорий ИКЛ', 'Категории ИКЛ');
    insert into modern.protect_function(id, name, note, caption) values (22, 'filterCard', 'Фильтр', 'Фильтр');

    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (0, 'tviНет данных', 1, null, 'root', 0);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (1, 'tviCards', 15, 0, 'Карточки', 1);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (2, 'tviCardFind', 2, 1, 'Найти карточку', 2);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (3, 'tviIklNew', 3, 1, 'Создать ИКЛ', 3);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (4, 'tviIk2New', 4, 1, 'Создать ИК-2', 4);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (5, 'tviListOpenCard', 17, 0, 'Список открытых карт', 100000);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (6, 'tviDict', 18, 0, 'Справочники', 101);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (7, 'tviMvd', 5, 6, 'Коды МВД', 102);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (8, 'tviLin', 6, 6, 'Коды райлинорганов', 103);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (9, 'tviDiv', 7, 6, 'Список подразделений', 104);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (10, 'tviPost', 8, 6, 'Должности', 105);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (11, 'tviExp', 9, 6, 'Эксперты', 106);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (12, 'tviMeth', 10, 6, 'Методы расчета', 107);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (13, 'tviUK', 11, 6, 'Статьи УК', 108);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (14, 'tviAdmin', 16, 0, 'Администрирование', 201);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (17, 'tviOrg', 14, 6, 'Организации', 109);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (15, 'tviRight', 12, 14, 'Назначение прав пользвателю', 202);
    insert into modern.controls (ID, NAME, FUNC_ID, PARENT_ID, CAPTION, SORT_ORD) values (16, 'tviRole', 13, 14, 'Роли', 203);
    insert into modern.controls(id, name, func_id, parent_id, caption, SORT_ORD) values (18, 'tviSort', 19, 6, 'Виды объекта', 110);
    insert into modern.controls(id, name, func_id, parent_id, caption, SORT_ORD) values (19, 'tviClassObject', 20, 6, 'Категории объекта', 111);
    insert into modern.controls(id, name, func_id, parent_id, caption, SORT_ORD) values (20, 'tviClassIkl', 21, 6, 'Категории ИКЛ', 112);
    insert into modern.controls(id, name, func_id, parent_id, caption, SORT_ORD) values (21, 'tviFilter', 22, 1, 'Выбрать карточки', 5);

    insert into modern.groups (ID, NAME, NOTE) values (1, 'Дерево основной формы', '');

    -- Создание роли администратора
    insert into modern.post(id, name) values (-1000, 'admin');
    insert into modern.division(id,name) values (-1000, 'admin');
    salt := dbms_random.string('a', 4);
    insert into modern.expert(id, division_id, post_id, surname, name, patronic, login, password, sign, salt)
      values(-1000, -1000, -1000, 'admin', 'admin', 'admin', 'admin', modern.PKG_SCR.getHash('admin'||salt), 'admin', salt);
    insert into modern.history(id, expert_id, action_id, date_insert, note) values
      (-1000, -1000, -1000, sysdate, 'Создание базы данных');
    insert into modern.roles(id, name) values (-1000, 'admin');
    insert into modern.action_roles(role_id, func_id) values (-1000, 1);
    insert into modern.action_roles(role_id, func_id) values (-1000, 5);
    insert into modern.action_roles(role_id, func_id) values (-1000, 6);
    insert into modern.action_roles(role_id, func_id) values (-1000, 7);
    insert into modern.action_roles(role_id, func_id) values (-1000, 8);
    insert into modern.action_roles(role_id, func_id) values (-1000, 9);
    insert into modern.action_roles(role_id, func_id) values (-1000, 10);
    insert into modern.action_roles(role_id, func_id) values (-1000, 11);
    insert into modern.action_roles(role_id, func_id) values (-1000, 12);
    insert into modern.action_roles(role_id, func_id) values (-1000, 13);
    insert into modern.action_roles(role_id, func_id) values (-1000, 14);
    insert into modern.action_roles(role_id, func_id) values (-1000, 16);
    insert into modern.action_roles(role_id, func_id) values (-1000, 18);
    insert into modern.action_roles(role_id, func_id) values (-1000, 19);
    insert into modern.action_roles(role_id, func_id) values (-1000, 20);
    insert into modern.action_roles(role_id, func_id) values (-1000, 21);

    insert into modern.action_group(res_id, func_id, id) values (1, 1, 1);
    insert into modern.action_group(res_id, func_id, id) values (2, 2, 1);
    insert into modern.action_group(res_id, func_id, id) values (3, 3, 1);
    insert into modern.action_group(res_id, func_id, id) values (4, 4, 1);
    insert into modern.action_group(res_id, func_id, id) values (5, 1, 1);
    insert into modern.action_group(res_id, func_id, id) values (6, 1, 1);
    insert into modern.action_group(res_id, func_id, id) values (7, 5, 1);
    insert into modern.action_group(res_id, func_id, id) values (8, 6, 1);
    insert into modern.action_group(res_id, func_id, id) values (9, 7, 1);
    insert into modern.action_group(res_id, func_id, id) values (10, 8, 1);
    insert into modern.action_group(res_id, func_id, id) values (11, 9, 1);
    insert into modern.action_group(res_id, func_id, id) values (12, 10, 1);
    insert into modern.action_group(res_id, func_id, id) values (13, 1, 1);
    insert into modern.action_group(res_id, func_id, id) values (14, 1, 1);
    insert into modern.action_group(res_id, func_id, id) values (15, 12, 1);
    insert into modern.action_group(res_id, func_id, id) values (16, 13, 1);
    insert into modern.action_group(res_id, func_id, id) values (17, 14, 1);
    insert into modern.action_group(id, res_id, func_id) values (1, 18, 19);
    insert into modern.action_group(id, res_id, func_id) values (1, 19, 20);
    insert into modern.action_group(id, res_id, func_id) values (1, 20, 21);
    insert into modern.action_group(id, res_id, func_id) values (1, 21, 22);

    insert into modern.roles_expert(role_id, expert_id) values(-1000, -1000);

    -- Создается метод расчета по умолчанию
    -- Задаем метод по умолчанию
    insert into modern.method(id, name, def_freq)values(1, 'Основной метод', 0.007);

    -- заполняется таблица локусов
    insert into modern.locus (id, name, history_id) values (1, 'CSF1PO', -1000);
    insert into modern.locus (id, name, history_id) values (2, 'D3S1358', -1000);
    insert into modern.locus (id, name, history_id)  values (3, 'D5S818', -1000);
    insert into modern.locus (id, name, history_id)  values (4, 'D7S820', -1000);
    insert into modern.locus (id, name, history_id)  values (5, 'D8S1179', -1000);
    insert into modern.locus (id, name, history_id)  values (6, 'D13S317', -1000);
    insert into modern.locus (id, name, history_id)  values (7, 'D16S539', -1000);
    insert into modern.locus (id, name, history_id)  values (8, 'D18S51', -1000);
    insert into modern.locus (id, name, history_id)  values (9, 'D21S11', -1000);
    insert into modern.locus (id, name, history_id)  values (10, 'FGA', -1000);
    insert into modern.locus (id, name, history_id)  values (11, 'TH01', -1000);
    insert into modern.locus (id, name, history_id)  values (12, 'TPOX', -1000);
    insert into modern.locus (id, name, history_id)  values (13, 'vWA', -1000);
    insert into modern.locus (id, name, history_id)  values (14, 'Amelogenin', -1000);
    insert into modern.locus (id, name, history_id)  values (15, 'D2S1338', -1000);
    insert into modern.locus (id, name, history_id)  values (16, 'D19S433', -1000);
    insert into modern.locus (id, name, history_id)  values (17, 'F13A01', -1000);
    insert into modern.locus (id, name, history_id)  values (18, 'F13B', -1000);
    insert into modern.locus (id, name, history_id)  values (19, 'FESFPS', -1000);
    insert into modern.locus (id, name, history_id)  values (20, 'LPL', -1000);
    insert into modern.locus (id, name, history_id)  values (21, 'HPRTB', -1000);
    insert into modern.locus (id, name, history_id)  values (22, 'Penta E', -1000);
    insert into modern.locus (id, name, history_id)  values (23, 'Penta D', -1000);
    insert into modern.locus (id, name, history_id)  values (24, 'SE33', -1000);

    -- заполняется таблица аллелей
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (111, 1, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (112, 1, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (113, 1, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (114, 1, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (115, 1, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (116, 1, '10.2', 10.2, -1000, 19);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (117, 1, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (118, 1, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (119, 1, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (120, 1, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (121, 1, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (122, 1, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (9, 2, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (10, 2, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (11, 2, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (12, 2, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (13, 2, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (14, 2, '15.2', 15.2, -1000, 31);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (15, 2, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (16, 2, '16.2', 16.2, -1000, 33);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (17, 2, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (18, 2, '17.2', 17.2, -1000, 35);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (19, 2, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (20, 2, '18.2', 18.2, -1000, 37);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (21, 2, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (22, 2, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (23, 2, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (123, 3, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (124, 3, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (125, 3, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (126, 3, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (127, 3, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (128, 3, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (129, 3, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (130, 3, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (131, 3, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (132, 3, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (133, 3, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (134, 3, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (145, 4, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (146, 4, '5.2', 5.2, -1000, 6);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (147, 4, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (148, 4, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (149, 4, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (150, 4, '8.2', 8.2, -1000, 13);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (151, 4, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (152, 4, '9.2', 9.2, -1000, 16);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (153, 4, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (154, 4, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (155, 4, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (156, 4, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (157, 4, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (158, 4, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (159, 4, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (440, 4, '11.1', 11.1, -1000, 92);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (160, 5, '7.3', 7.3, -1000, 11);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (161, 5, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (162, 5, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (163, 5, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (164, 5, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (165, 5, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (166, 5, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (167, 5, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (168, 5, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (169, 5, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (170, 5, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (171, 5, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (172, 5, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (173, 5, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (135, 6, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (136, 6, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (137, 6, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (138, 6, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (139, 6, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (140, 6, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (141, 6, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (142, 6, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (143, 6, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (144, 6, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (245, 7, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (246, 7, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (247, 7, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (248, 7, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (249, 7, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (250, 7, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (251, 7, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (252, 7, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (253, 7, '12.2', 12.2, -1000, 24);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (254, 7, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (255, 7, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (256, 7, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (257, 7, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (206, 8, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (207, 8, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (208, 8, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (209, 8, '9.2', 9.2, -1000, 16);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (210, 8, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (211, 8, '10.2', 10.2, -1000, 19);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (212, 8, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (213, 8, '11.2', 11.2, -1000, 22);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (214, 8, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (215, 8, '12.2', 12.2, -1000, 24);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (216, 8, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (217, 8, '13.2', 13.2, -1000, 26);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (218, 8, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (219, 8, '14.2', 14.2, -1000, 29);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (220, 8, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (221, 8, '15.2', 15.2, -1000, 31);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (222, 8, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (223, 8, '16.2', 16.2, -1000, 33);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (224, 8, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (225, 8, '17.2', 17.2, -1000, 35);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (226, 8, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (227, 8, '18.2', 18.2, -1000, 37);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (228, 8, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (229, 8, '19.2', 19.2, -1000, 39);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (230, 8, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (231, 8, '20.2', 20.2, -1000, 41);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (232, 8, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (233, 8, '21.2', 21.2, -1000, 43);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (234, 8, '22', 22, -1000, 44);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (235, 8, '22.2', 22.2, -1000, 45);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (236, 8, '23', 23, -1000, 46);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (237, 8, '23.2', 23.2, -1000, 47);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (238, 8, '24', 24, -1000, 48);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (239, 8, '24.2', 24.2, -1000, 49);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (240, 8, '25', 25, -1000, 50);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (241, 8, '25.2', 25.2, -1000, 51);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (242, 8, '26', 26, -1000, 52);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (243, 8, '26.2', 26.2, -1000, 53);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (244, 8, '27', 27, -1000, 54);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (174, 9, '23.2', 23.2, -1000, 47);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (175, 9, '24', 24, -1000, 48);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (176, 9, '24.2', 24.2, -1000, 49);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (177, 9, '25', 25, -1000, 50);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (178, 9, '25.2', 25.2, -1000, 51);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (179, 9, '26', 26, -1000, 52);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (180, 9, '26.2', 26.2, -1000, 53);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (181, 9, '27', 27, -1000, 54);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (182, 9, '27.2', 27.2, -1000, 55);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (183, 9, '28', 28, -1000, 56);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (184, 9, '28.2', 28.2, -1000, 57);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (185, 9, '29', 29, -1000, 58);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (186, 9, '29.2', 29.2, -1000, 59);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (187, 9, '30', 30, -1000, 60);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (188, 9, '30.2', 30.2, -1000, 61);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (189, 9, '31', 31, -1000, 62);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (190, 9, '31.2', 31.2, -1000, 63);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (191, 9, '32', 32, -1000, 64);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (192, 9, '32.2', 32.2, -1000, 65);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (193, 9, '33', 33, -1000, 66);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (194, 9, '33.2', 33.2, -1000, 67);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (195, 9, '34', 34, -1000, 68);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (196, 9, '34.2', 34.2, -1000, 69);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (197, 9, '35', 35, -1000, 70);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (198, 9, '35.2', 35.2, -1000, 71);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (199, 9, '36', 36, -1000, 72);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (200, 9, '36.2', 36.2, -1000, 73);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (201, 9, '37', 37, -1000, 74);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (202, 9, '37.2', 37.2, -1000, 75);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (203, 9, '38', 38, -1000, 76);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (204, 9, '38.2', 38.2, -1000, 77);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (205, 9, '39', 39, -1000, 78);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (39, 10, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (40, 10, '17.2', 17.2, -1000, 35);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (41, 10, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (42, 10, '18.2', 18.2, -1000, 37);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (43, 10, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (44, 10, '19.2', 19.2, -1000, 39);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (45, 10, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (46, 10, '20.2', 20.2, -1000, 41);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (47, 10, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (48, 10, '21.2', 21.2, -1000, 43);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (49, 10, '22', 22, -1000, 44);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (50, 10, '22.2', 22.2, -1000, 45);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (51, 10, '23', 23, -1000, 46);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (52, 10, '23.2', 23.2, -1000, 47);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (53, 10, '24', 24, -1000, 48);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (54, 10, '24.2', 24.2, -1000, 49);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (55, 10, '25', 25, -1000, 50);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (56, 10, '25.2', 25.2, -1000, 51);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (57, 10, '26', 26, -1000, 52);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (58, 10, '26.2', 26.2, -1000, 53);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (59, 10, '27', 27, -1000, 54);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (60, 10, '27.2', 27.2, -1000, 55);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (61, 10, '28', 28, -1000, 56);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (62, 10, '28.2', 28.2, -1000, 57);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (63, 10, '29', 29, -1000, 58);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (64, 10, '29.2', 29.2, -1000, 59);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (65, 10, '30', 30, -1000, 60);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (66, 10, '30.2', 30.2, -1000, 61);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (67, 10, '31', 31, -1000, 62);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (68, 10, '31.2', 31.2, -1000, 63);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (69, 10, '43.2', 43.2, -1000, 80);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (70, 10, '44.2', 44.2, -1000, 81);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (71, 10, '45.2', 45.2, -1000, 82);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (72, 10, '46.2', 46.2, -1000, 83);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (73, 10, '47.2', 47.2, -1000, 84);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (74, 10, '48.2', 48.2, -1000, 85);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (75, 10, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (76, 10, '16.2', 16.2, -1000, 33);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (77, 10, '32', 32, -1000, 64);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (78, 10, '32.2', 32.2, -1000, 65);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (79, 10, '33.2', 33.2, -1000, 67);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (80, 10, '34.2', 34.2, -1000, 69);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (81, 10, '42.2', 42.2, -1000, 79);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (82, 10, '49.2', 49.2, -1000, 86);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (83, 10, '50.2', 50.2, -1000, 87);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (86, 11, '4', 4, -1000, 3);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (87, 11, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (88, 11, '5.3', 5.3, -1000, 7);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (89, 11, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (90, 11, '6.3', 6.3, -1000, 9);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (91, 11, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (92, 11, '7.3', 7.3, -1000, 11);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (93, 11, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (94, 11, '8.3', 8.3, -1000, 14);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (95, 11, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (96, 11, '9.3', 9.3, -1000, 17);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (97, 11, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (98, 11, '10.3', 10.3, -1000, 20);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (99, 11, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (100, 11, '13.3', 13.3, -1000, 27);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (101, 12, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (102, 12, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (103, 12, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (104, 12, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (105, 12, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (106, 12, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (107, 12, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (108, 12, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (109, 12, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (110, 12, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (24, 13, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (25, 13, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (26, 13, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (27, 13, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (28, 13, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (29, 13, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (30, 13, '15.2', 15.2, -1000, 31);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (31, 13, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (32, 13, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (33, 13, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (34, 13, '18.2', 18.2, -1000, 37);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (35, 13, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (36, 13, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (37, 13, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (38, 13, '22', 22, -1000, 44);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (84, 14, 'X', 0, -1000, 1);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (85, 14, 'Y', 1, -1000, 2);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (258, 15, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (259, 15, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (260, 15, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (261, 15, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (262, 15, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (263, 15, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (264, 15, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (265, 15, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (266, 15, '22', 22, -1000, 44);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (267, 15, '23', 23, -1000, 46);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (268, 15, '24', 24, -1000, 48);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (269, 15, '25', 25, -1000, 50);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (270, 15, '26', 26, -1000, 52);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (271, 15, '27', 27, -1000, 54);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (272, 15, '28', 28, -1000, 56);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (273, 15, '29', 29, -1000, 58);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (274, 16, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (275, 16, '9.2', 9.2, -1000, 16);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (276, 16, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (277, 16, '10.2', 10.2, -1000, 19);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (278, 16, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (279, 16, '11.2', 11.2, -1000, 22);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (280, 16, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (281, 16, '12.2', 12.2, -1000, 24);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (282, 16, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (283, 16, '13.2', 13.2, -1000, 26);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (284, 16, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (285, 16, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (286, 16, '15.2', 15.2, -1000, 31);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (287, 16, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (288, 16, '16.2', 16.2, -1000, 33);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (289, 16, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (290, 16, '17.2', 17.2, -1000, 35);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (291, 16, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (292, 16, '18.2', 18.2, -1000, 37);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (439, 16, '14.2', 14.2, -1000, 29);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (413, 17, '3.2', 3.2, -1000, 91);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (414, 17, '4', 4, -1000, 3);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (415, 17, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (416, 17, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (417, 17, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (418, 17, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (419, 17, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (420, 17, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (421, 17, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (422, 17, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (423, 17, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (424, 17, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (425, 17, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (426, 17, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (396, 18, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (397, 18, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (398, 18, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (399, 18, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (400, 18, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (401, 18, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (402, 18, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (403, 18, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (404, 18, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (405, 19, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (406, 19, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (407, 19, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (408, 19, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (409, 19, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (410, 19, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (411, 19, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (412, 19, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (1, 20, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (2, 20, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (3, 20, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (4, 20, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (5, 20, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (6, 20, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (7, 20, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (8, 20, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (427, 21, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (428, 21, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (429, 21, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (430, 21, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (431, 21, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (432, 21, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (433, 21, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (434, 21, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (435, 21, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (436, 21, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (437, 21, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (438, 21, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (360, 22, '4', 4, -1000, 3);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (361, 22, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (362, 22, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (363, 22, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (364, 22, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (365, 22, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (366, 22, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (367, 22, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (368, 22, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (369, 22, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (370, 22, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (371, 22, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (372, 22, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (373, 22, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (374, 22, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (375, 22, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (376, 22, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (377, 22, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (378, 22, '22', 22, -1000, 44);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (379, 22, '23', 23, -1000, 46);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (380, 22, '24', 24, -1000, 48);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (381, 22, '25', 25, -1000, 50);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (382, 23, '2.2', 2.2, -1000, 89);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (383, 23, '3.2', 3.2, -1000, 91);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (384, 23, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (385, 23, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (386, 23, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (387, 23, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (388, 23, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (389, 23, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (390, 23, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (391, 23, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (392, 23, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (393, 23, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (394, 23, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (395, 23, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (357, 24, '37.2', 37.2, -1000, 75);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (358, 24, '38', 38, -1000, 76);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (359, 24, '7.2', 7.2, -1000, 90);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (293, 24, '4.2', 4.2, -1000, 4);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (294, 24, '5', 5, -1000, 5);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (295, 24, '6', 6, -1000, 8);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (296, 24, '6.3', 6.3, -1000, 9);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (297, 24, '7', 7, -1000, 10);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (298, 24, '8', 8, -1000, 12);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (299, 24, '8.2', 8.2, -1000, 13);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (300, 24, '9', 9, -1000, 15);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (301, 24, '9.2', 9.2, -1000, 16);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (302, 24, '10', 10, -1000, 18);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (303, 24, '10.2', 10.2, -1000, 19);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (304, 24, '11', 11, -1000, 21);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (305, 24, '11.2', 11.2, -1000, 22);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (306, 24, '12', 12, -1000, 23);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (307, 24, '12.2', 12.2, -1000, 24);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (308, 24, '13', 13, -1000, 25);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (309, 24, '13.2', 13.2, -1000, 26);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (310, 24, '14', 14, -1000, 28);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (311, 24, '14.2', 14.2, -1000, 29);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (312, 24, '15', 15, -1000, 30);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (313, 24, '15.2', 15.2, -1000, 31);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (314, 24, '16', 16, -1000, 32);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (315, 24, '16.2', 16.2, -1000, 33);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (316, 24, '17', 17, -1000, 34);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (317, 24, '17.2', 17.2, -1000, 35);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (318, 24, '18', 18, -1000, 36);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (319, 24, '18.2', 18.2, -1000, 37);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (320, 24, '19', 19, -1000, 38);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (321, 24, '19.2', 19.2, -1000, 39);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (322, 24, '20', 20, -1000, 40);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (323, 24, '20.2', 20.2, -1000, 41);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (324, 24, '21', 21, -1000, 42);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (325, 24, '21.2', 21.2, -1000, 43);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (326, 24, '22', 22, -1000, 44);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (327, 24, '22.2', 22.2, -1000, 45);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (328, 24, '23', 23, -1000, 46);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (329, 24, '23.2', 23.2, -1000, 47);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (330, 24, '24', 24, -1000, 48);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (331, 24, '24.2', 24.2, -1000, 49);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (332, 24, '25', 25, -1000, 50);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (333, 24, '25.2', 25.2, -1000, 51);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (334, 24, '26', 26, -1000, 52);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (335, 24, '26.2', 26.2, -1000, 53);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (336, 24, '27', 27, -1000, 54);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (337, 24, '27.2', 27.2, -1000, 55);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (338, 24, '28', 28, -1000, 56);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (339, 24, '28.2', 28.2, -1000, 57);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (340, 24, '29', 29, -1000, 58);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (341, 24, '29.2', 29.2, -1000, 59);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (342, 24, '30', 30, -1000, 60);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (343, 24, '30.2', 30.2, -1000, 61);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (344, 24, '31', 31, -1000, 62);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (345, 24, '31.2', 31.2, -1000, 63);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (346, 24, '32', 32, -1000, 64);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (347, 24, '32.2', 32.2, -1000, 65);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (348, 24, '33', 33, -1000, 66);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (349, 24, '33.2', 33.2, -1000, 67);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (350, 24, '34', 34, -1000, 68);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (351, 24, '34.2', 34.2, -1000, 69);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (352, 24, '35', 35, -1000, 70);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (353, 24, '35.2', 35.2, -1000, 71);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (354, 24, '36', 36, -1000, 72);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (355, 24, '36.2', 36.2, -1000, 73);
    insert into modern.allele (id, locus_id, name, val, history_id, old_allele_id) values (356, 24, '37', 37, -1000, 74);

    -- [LPL]
    insert into modern.allele_freq(allele_id, method_id, freq) values (318, 1, 0.001);
    insert into modern.allele_freq(allele_id, method_id, freq) values (319, 1, 0.001);
    insert into modern.allele_freq(allele_id, method_id, freq) values (320, 1, 0.061);
    insert into modern.allele_freq(allele_id, method_id, freq) values (321, 1, 0.439);
    insert into modern.allele_freq(allele_id, method_id, freq) values (322, 1, 0.236);
    insert into modern.allele_freq(allele_id, method_id, freq) values (323, 1, 0.235);
    insert into modern.allele_freq(allele_id, method_id, freq) values (324, 1, 0.026);
    insert into modern.allele_freq(allele_id, method_id, freq) values (325, 1, 0.002);
    --[F13B]
    insert into modern.allele_freq(allele_id, method_id, freq) values (301, 1, 0.073);
    insert into modern.allele_freq(allele_id, method_id, freq) values (302, 1, 0.008);
    insert into modern.allele_freq(allele_id, method_id, freq) values (303, 1, 0.227);
    insert into modern.allele_freq(allele_id, method_id, freq) values (304, 1, 0.249);
    insert into modern.allele_freq(allele_id, method_id, freq) values (305, 1, 0.432);
    insert into modern.allele_freq(allele_id, method_id, freq) values (306, 1, 0.007);
    insert into modern.allele_freq(allele_id, method_id, freq) values (307, 1, 0.002);
    --[FESFPS]
    insert into modern.allele_freq(allele_id, method_id, freq) values (310, 1, 0.003);
    insert into modern.allele_freq(allele_id, method_id, freq) values (311, 1, 0.015);
    insert into modern.allele_freq(allele_id, method_id, freq) values (312, 1, 0.008);
    insert into modern.allele_freq(allele_id, method_id, freq) values (313, 1, 0.249);
    insert into modern.allele_freq(allele_id, method_id, freq) values (314, 1, 0.473);
    insert into modern.allele_freq(allele_id, method_id, freq) values (315, 1, 0.192);
    insert into modern.allele_freq(allele_id, method_id, freq) values (316, 1, 0.057);
    insert into modern.allele_freq(allele_id, method_id, freq) values (317, 1, 0.004);
    --[F13A01]
    insert into modern.allele_freq(allele_id, method_id, freq) values (300, 1, 0.085);
    insert into modern.allele_freq(allele_id, method_id, freq) values (287, 1, 0.048);
    insert into modern.allele_freq(allele_id, method_id, freq) values (288, 1, 0.179);
    insert into modern.allele_freq(allele_id, method_id, freq) values (289, 1, 0.333);
    insert into modern.allele_freq(allele_id, method_id, freq) values (290, 1, 0.323);
    insert into modern.allele_freq(allele_id, method_id, freq) values (291, 1, 0.011);
    insert into modern.allele_freq(allele_id, method_id, freq) values (296, 1, 0.002);
    insert into modern.allele_freq(allele_id, method_id, freq) values (297, 1, 0.003);
    insert into modern.allele_freq(allele_id, method_id, freq) values (298, 1, 0.011);
    insert into modern.allele_freq(allele_id, method_id, freq) values (299, 1, 0.005);
    --[vWA]
    insert into modern.allele_freq(allele_id, method_id, freq) values (237, 1, 0.003);
    insert into modern.allele_freq(allele_id, method_id, freq) values (238, 1, 0.097);
    insert into modern.allele_freq(allele_id, method_id, freq) values (239, 1, 0.106);
    insert into modern.allele_freq(allele_id, method_id, freq) values (241, 1, 0.218);
    insert into modern.allele_freq(allele_id, method_id, freq) values (242, 1, 0.275);
    insert into modern.allele_freq(allele_id, method_id, freq) values (243, 1, 0.225);
    insert into modern.allele_freq(allele_id, method_id, freq) values (245, 1, 0.067);
    insert into modern.allele_freq(allele_id, method_id, freq) values (246, 1, 0.011);
    --[TH01]
    insert into modern.allele_freq(allele_id, method_id, freq) values (212, 1, 0.220);
    insert into modern.allele_freq(allele_id, method_id, freq) values (214, 1, 0.152);
    insert into modern.allele_freq(allele_id, method_id, freq) values (216, 1, 0.112);
    insert into modern.allele_freq(allele_id, method_id, freq) values (217, 1, 0.007);
    insert into modern.allele_freq(allele_id, method_id, freq) values (218, 1, 0.198);
    insert into modern.allele_freq(allele_id, method_id, freq) values (219, 1, 0.309);
    insert into modern.allele_freq(allele_id, method_id, freq) values (220, 1, 0.008);
    insert into modern.allele_freq(allele_id, method_id, freq) values (222, 1, 0.002);
    --[TPOX]
    insert into modern.allele_freq(allele_id, method_id, freq) values (225, 1, 0.004);
    insert into modern.allele_freq(allele_id, method_id, freq) values (226, 1, 0.001);
    insert into modern.allele_freq(allele_id, method_id, freq) values (227, 1, 0.537);
    insert into modern.allele_freq(allele_id, method_id, freq) values (228, 1, 0.100);
    insert into modern.allele_freq(allele_id, method_id, freq) values (229, 1, 0.069);
    insert into modern.allele_freq(allele_id, method_id, freq) values (230, 1, 0.253);
    insert into modern.allele_freq(allele_id, method_id, freq) values (231, 1, 0.035);
    --[CSF1PO]
    insert into modern.allele_freq(allele_id, method_id, freq) values (2, 1, 0.007);
    insert into modern.allele_freq(allele_id, method_id, freq) values (3, 1, 0.003);
    insert into modern.allele_freq(allele_id, method_id, freq) values (4, 1, 0.077);
    insert into modern.allele_freq(allele_id, method_id, freq) values (5, 1, 0.283);
    insert into modern.allele_freq(allele_id, method_id, freq) values (7, 1, 0.281);
    insert into modern.allele_freq(allele_id, method_id, freq) values (8, 1, 0.272);
    insert into modern.allele_freq(allele_id, method_id, freq) values (9, 1, 0.069);
    insert into modern.allele_freq(allele_id, method_id, freq) values (10, 1, 0.014);
    insert into modern.allele_freq(allele_id, method_id, freq) values (11, 1, 0.002);
    --[D16S539]
    insert into modern.allele_freq(allele_id, method_id, freq) values (80, 1, 0.007);
    insert into modern.allele_freq(allele_id, method_id, freq) values (83, 1, 0.008);
    insert into modern.allele_freq(allele_id, method_id, freq) values (84, 1, 0.122);
    insert into modern.allele_freq(allele_id, method_id, freq) values (85, 1, 0.097);
    insert into modern.allele_freq(allele_id, method_id, freq) values (86, 1, 0.268);
    insert into modern.allele_freq(allele_id, method_id, freq) values (89, 1, 0.278);
    insert into modern.allele_freq(allele_id, method_id, freq) values (90, 1, 0.192);
    insert into modern.allele_freq(allele_id, method_id, freq) values (91, 1, 0.035);
    --[D7S820]
    insert into modern.allele_freq(allele_id, method_id, freq) values (42, 1, 0.002);
    insert into modern.allele_freq(allele_id, method_id, freq) values (43, 1, 0.016);
    insert into modern.allele_freq(allele_id, method_id, freq) values (44, 1, 0.141);
    insert into modern.allele_freq(allele_id, method_id, freq) values (46, 1, 0.141);
    insert into modern.allele_freq(allele_id, method_id, freq) values (48, 1, 0.223);
    insert into modern.allele_freq(allele_id, method_id, freq) values (49, 1, 0.239);
    insert into modern.allele_freq(allele_id, method_id, freq) values (55, 1, 0.002);
    insert into modern.allele_freq(allele_id, method_id, freq) values (50, 1, 0.193);
    insert into modern.allele_freq(allele_id, method_id, freq) values (51, 1, 0.035);
    insert into modern.allele_freq(allele_id, method_id, freq) values (52, 1, 0.005);
    --[D13S317]
    insert into modern.allele_freq(allele_id, method_id, freq) values (71, 1, 0.161);
    insert into modern.allele_freq(allele_id, method_id, freq) values (72, 1, 0.083);
    insert into modern.allele_freq(allele_id, method_id, freq) values (73, 1, 0.048);
    insert into modern.allele_freq(allele_id, method_id, freq) values (74, 1, 0.341);
    insert into modern.allele_freq(allele_id, method_id, freq) values (75, 1, 0.247);
    insert into modern.allele_freq(allele_id, method_id, freq) values (76, 1, 0.070);
    insert into modern.allele_freq(allele_id, method_id, freq) values (77, 1, 0.048);
    --[D3S1358]
    insert into modern.allele_freq(allele_id, method_id, freq) values (15, 1, 0.007);
    insert into modern.allele_freq(allele_id, method_id, freq) values (16, 1, 0.158);
    insert into modern.allele_freq(allele_id, method_id, freq) values (17, 1, 0.254);
    insert into modern.allele_freq(allele_id, method_id, freq) values (19, 1, 0.228);
    insert into modern.allele_freq(allele_id, method_id, freq) values (21, 1, 0.182);
    insert into modern.allele_freq(allele_id, method_id, freq) values (23, 1, 0.165);
    insert into modern.allele_freq(allele_id, method_id, freq) values (25, 1, 0.010);
    --[D2S1338]
    insert into modern.allele_freq(allele_id, method_id, freq) values (253, 1, 0.047);
    insert into modern.allele_freq(allele_id, method_id, freq) values (254, 1, 0.173);
    insert into modern.allele_freq(allele_id, method_id, freq) values (255, 1, 0.063);
    insert into modern.allele_freq(allele_id, method_id, freq) values (256, 1, 0.138);
    insert into modern.allele_freq(allele_id, method_id, freq) values (257, 1, 0.146);
    insert into modern.allele_freq(allele_id, method_id, freq) values (258, 1, 0.026);
    insert into modern.allele_freq(allele_id, method_id, freq) values (259, 1, 0.040);
    insert into modern.allele_freq(allele_id, method_id, freq) values (260, 1, 0.115);
    insert into modern.allele_freq(allele_id, method_id, freq) values (261, 1, 0.118);
    insert into modern.allele_freq(allele_id, method_id, freq) values (262, 1, 0.106);
    insert into modern.allele_freq(allele_id, method_id, freq) values (263, 1, 0.027);
    insert into modern.allele_freq(allele_id, method_id, freq) values (264, 1, 0.007);
    --[D8S1179]
    insert into modern.allele_freq(allele_id, method_id, freq) values (57, 1, 0.023);
    insert into modern.allele_freq(allele_id, method_id, freq) values (58, 1, 0.012);
    insert into modern.allele_freq(allele_id, method_id, freq) values (59, 1, 0.097);
    insert into modern.allele_freq(allele_id, method_id, freq) values (60, 1, 0.060);
    insert into modern.allele_freq(allele_id, method_id, freq) values (61, 1, 0.140);
    insert into modern.allele_freq(allele_id, method_id, freq) values (62, 1, 0.325);
    insert into modern.allele_freq(allele_id, method_id, freq) values (63, 1, 0.214);
    insert into modern.allele_freq(allele_id, method_id, freq) values (64, 1, 0.099);
    insert into modern.allele_freq(allele_id, method_id, freq) values (65, 1, 0.027);
    insert into modern.allele_freq(allele_id, method_id, freq) values (66, 1, 0.007);
    --[SE33]
    insert into modern.allele_freq(allele_id, method_id, freq) values (387, 1, 0.022);
    insert into modern.allele_freq(allele_id, method_id, freq) values (389, 1, 0.029);
    insert into modern.allele_freq(allele_id, method_id, freq) values (391, 1, 0.036);
    insert into modern.allele_freq(allele_id, method_id, freq) values (393, 1, 0.036);
    insert into modern.allele_freq(allele_id, method_id, freq) values (395, 1, 0.051);
    insert into modern.allele_freq(allele_id, method_id, freq) values (397, 1, 0.101);
    insert into modern.allele_freq(allele_id, method_id, freq) values (399, 1, 0.073);
    insert into modern.allele_freq(allele_id, method_id, freq) values (401, 1, 0.080);
    insert into modern.allele_freq(allele_id, method_id, freq) values (403, 1, 0.029);
    insert into modern.allele_freq(allele_id, method_id, freq) values (405, 1, 0.051);
    insert into modern.allele_freq(allele_id, method_id, freq) values (408, 1, 0.044);
    insert into modern.allele_freq(allele_id, method_id, freq) values (410, 1, 0.036);
    insert into modern.allele_freq(allele_id, method_id, freq) values (414, 1, 0.051);
    insert into modern.allele_freq(allele_id, method_id, freq) values (416, 1, 0.044);
    insert into modern.allele_freq(allele_id, method_id, freq) values (418, 1, 0.008);
    insert into modern.allele_freq(allele_id, method_id, freq) values (420, 1, 0.094);
    insert into modern.allele_freq(allele_id, method_id, freq) values (422, 1, 0.044);
    insert into modern.allele_freq(allele_id, method_id, freq) values (424, 1, 0.065);
    insert into modern.allele_freq(allele_id, method_id, freq) values (426, 1, 0.022);
    insert into modern.allele_freq(allele_id, method_id, freq) values (428, 1, 0.007);
    --[D19S433]
    insert into modern.allele_freq(allele_id, method_id, freq) values (273, 1, 0.077);
    insert into modern.allele_freq(allele_id, method_id, freq) values (275, 1, 0.289);
    insert into modern.allele_freq(allele_id, method_id, freq) values (276, 1, 0.017);
    insert into modern.allele_freq(allele_id, method_id, freq) values (277, 1, 0.341);
    insert into modern.allele_freq(allele_id, method_id, freq) values (278, 1, 0.009);
    insert into modern.allele_freq(allele_id, method_id, freq) values (279, 1, 0.158);
    insert into modern.allele_freq(allele_id, method_id, freq) values (280, 1, 0.027);
    insert into modern.allele_freq(allele_id, method_id, freq) values (281, 1, 0.042);
    insert into modern.allele_freq(allele_id, method_id, freq) values (282, 1, 0.017);
    insert into modern.allele_freq(allele_id, method_id, freq) values (283, 1, 0.007);
    --[FGA]
    insert into modern.allele_freq(allele_id, method_id, freq) values (168, 1, 0.027);
    insert into modern.allele_freq(allele_id, method_id, freq) values (170, 1, 0.062);
    insert into modern.allele_freq(allele_id, method_id, freq) values (172, 1, 0.139);
    insert into modern.allele_freq(allele_id, method_id, freq) values (174, 1, 0.169);
    insert into modern.allele_freq(allele_id, method_id, freq) values (176, 1, 0.169);
    insert into modern.allele_freq(allele_id, method_id, freq) values (177, 1, 0.013);
    insert into modern.allele_freq(allele_id, method_id, freq) values (178, 1, 0.152);
    insert into modern.allele_freq(allele_id, method_id, freq) values (180, 1, 0.138);
    insert into modern.allele_freq(allele_id, method_id, freq) values (182, 1, 0.086);
    insert into modern.allele_freq(allele_id, method_id, freq) values (184, 1, 0.027);
    --[D21S11]
    insert into modern.allele_freq(allele_id, method_id, freq) values (139, 1, 0.046);
    insert into modern.allele_freq(allele_id, method_id, freq) values (141, 1, 0.168);
    insert into modern.allele_freq(allele_id, method_id, freq) values (143, 1, 0.205);
    insert into modern.allele_freq(allele_id, method_id, freq) values (145, 1, 0.252);
    insert into modern.allele_freq(allele_id, method_id, freq) values (146, 1, 0.033);
    insert into modern.allele_freq(allele_id, method_id, freq) values (147, 1, 0.072);
    insert into modern.allele_freq(allele_id, method_id, freq) values (148, 1, 0.095);
    insert into modern.allele_freq(allele_id, method_id, freq) values (149, 1, 0.014);
    insert into modern.allele_freq(allele_id, method_id, freq) values (150, 1, 0.072);
    insert into modern.allele_freq(allele_id, method_id, freq) values (152, 1, 0.033);
    --[D18S51]
    insert into modern.allele_freq(allele_id, method_id, freq) values (97, 1, 0.009);
    insert into modern.allele_freq(allele_id, method_id, freq) values (99, 1, 0.012);
    insert into modern.allele_freq(allele_id, method_id, freq) values (101, 1, 0.139);
    insert into modern.allele_freq(allele_id, method_id, freq) values (103, 1, 0.122);
    insert into modern.allele_freq(allele_id, method_id, freq) values (105, 1, 0.168);
    insert into modern.allele_freq(allele_id, method_id, freq) values (107, 1, 0.136);
    insert into modern.allele_freq(allele_id, method_id, freq) values (109, 1, 0.136);
    insert into modern.allele_freq(allele_id, method_id, freq) values (111, 1, 0.123);
    insert into modern.allele_freq(allele_id, method_id, freq) values (113, 1, 0.077);
    insert into modern.allele_freq(allele_id, method_id, freq) values (115, 1, 0.044);
    insert into modern.allele_freq(allele_id, method_id, freq) values (117, 1, 0.017);
    insert into modern.allele_freq(allele_id, method_id, freq) values (119, 1, 0.010);
    --[D5S818]
    insert into modern.allele_freq(allele_id, method_id, freq) values (31, 1, 0.042);
    insert into modern.allele_freq(allele_id, method_id, freq) values (32, 1, 0.054);
    insert into modern.allele_freq(allele_id, method_id, freq) values (33, 1, 0.393);
    insert into modern.allele_freq(allele_id, method_id, freq) values (34, 1, 0.352);
    insert into modern.allele_freq(allele_id, method_id, freq) values (35, 1, 0.158);
    insert into modern.allele_freq(allele_id, method_id, freq) values (36, 1, 0.007);

    -- Создание статей УК
    insert into modern.ukitem(id, hash)values(0, pkg_scr.gethash('null'));
    insert into modern.ukitem(id, parent_id, hash) values (1, 0, modern.pkg_scr.gethash('105'));
    insert into modern.ukitem(id, parent_id, hash) values (2, 0, modern.pkg_scr.gethash('106'));
    insert into modern.ukitem(id, parent_id, hash) values (3, 0, modern.pkg_scr.gethash('107'));
    insert into modern.ukitem(id, parent_id, hash) values (5, 0, modern.pkg_scr.gethash('109'));
    insert into modern.ukitem(id, parent_id, hash) values (6, 0, modern.pkg_scr.gethash('110'));
    insert into modern.ukitem(id, parent_id, hash) values (7, 0, modern.pkg_scr.gethash('111'));
    insert into modern.ukitem(id, parent_id, hash) values (8, 0, modern.pkg_scr.gethash('112'));
    insert into modern.ukitem(id, parent_id, hash) values (10, 0, modern.pkg_scr.gethash('113'));
    insert into modern.ukitem(id, parent_id, hash) values (12, 0, modern.pkg_scr.gethash('115'));
    insert into modern.ukitem(id, parent_id, hash) values (13, 0, modern.pkg_scr.gethash('116'));
    insert into modern.ukitem(id, parent_id, hash) values (14, 0, modern.pkg_scr.gethash('117'));
    insert into modern.ukitem(id, parent_id, hash) values (16, 0, modern.pkg_scr.gethash('118'));
    insert into modern.ukitem(id, parent_id, hash) values (17, 0, modern.pkg_scr.gethash('119'));
    insert into modern.ukitem(id, parent_id, hash) values (18, 0, modern.pkg_scr.gethash('120'));
    insert into modern.ukitem(id, parent_id, hash) values (19, 0, modern.pkg_scr.gethash('121'));
    insert into modern.ukitem(id, parent_id, hash) values (20, 0, modern.pkg_scr.gethash('122'));
    insert into modern.ukitem(id, parent_id, hash) values (21, 0, modern.pkg_scr.gethash('123'));
    insert into modern.ukitem(id, parent_id, hash) values (22, 0, modern.pkg_scr.gethash('124'));
    insert into modern.ukitem(id, parent_id, hash) values (23, 0, modern.pkg_scr.gethash('125'));
    insert into modern.ukitem(id, parent_id, hash) values (24, 0, modern.pkg_scr.gethash('126'));
    insert into modern.ukitem(id, parent_id, hash) values (25, 0, modern.pkg_scr.gethash('127'));
    insert into modern.ukitem(id, parent_id, hash) values (26, 0, modern.pkg_scr.gethash('127.1'));
    insert into modern.ukitem(id, parent_id, hash) values (27, 0, modern.pkg_scr.gethash('127.2'));
    insert into modern.ukitem(id, parent_id, hash) values (28, 0, modern.pkg_scr.gethash('128'));
    insert into modern.ukitem(id, parent_id, hash) values (30, 0, modern.pkg_scr.gethash('129'));
    insert into modern.ukitem(id, parent_id, hash) values (31, 0, modern.pkg_scr.gethash('130'));
    insert into modern.ukitem(id, parent_id, hash) values (32, 0, modern.pkg_scr.gethash('131'));
    insert into modern.ukitem(id, parent_id, hash) values (33, 0, modern.pkg_scr.gethash('132'));
    insert into modern.ukitem(id, parent_id, hash) values (34, 0, modern.pkg_scr.gethash('133'));
    insert into modern.ukitem(id, parent_id, hash) values (35, 0, modern.pkg_scr.gethash('134'));
    insert into modern.ukitem(id, parent_id, hash) values (36, 0, modern.pkg_scr.gethash('135'));
    insert into modern.ukitem(id, parent_id, hash) values (37, 0, modern.pkg_scr.gethash('136'));
    insert into modern.ukitem(id, parent_id, hash) values (38, 0, modern.pkg_scr.gethash('137'));
    insert into modern.ukitem(id, parent_id, hash) values (40, 0, modern.pkg_scr.gethash('139'));
    insert into modern.ukitem(id, parent_id, hash) values (41, 0, modern.pkg_scr.gethash('140'));
    insert into modern.ukitem(id, parent_id, hash) values (43, 0, modern.pkg_scr.gethash('141.1'));
    insert into modern.ukitem(id, parent_id, hash) values (44, 0, modern.pkg_scr.gethash('142'));
    insert into modern.ukitem(id, parent_id, hash) values (45, 0, modern.pkg_scr.gethash('142.1'));
    insert into modern.ukitem(id, parent_id, hash) values (46, 0, modern.pkg_scr.gethash('143'));
    insert into modern.ukitem(id, parent_id, hash) values (47, 0, modern.pkg_scr.gethash('144'));
    insert into modern.ukitem(id, parent_id, hash) values (49, 0, modern.pkg_scr.gethash('145.1'));
    insert into modern.ukitem(id, parent_id, hash) values (50, 0, modern.pkg_scr.gethash('146'));
    insert into modern.ukitem(id, parent_id, hash) values (51, 0, modern.pkg_scr.gethash('147'));
    insert into modern.ukitem(id, parent_id, hash) values (52, 0, modern.pkg_scr.gethash('148'));
    insert into modern.ukitem(id, parent_id, hash) values (54, 0, modern.pkg_scr.gethash('150'));
    insert into modern.ukitem(id, parent_id, hash) values (56, 0, modern.pkg_scr.gethash('151'));
    insert into modern.ukitem(id, parent_id, hash) values (57, 0, modern.pkg_scr.gethash('153'));
    insert into modern.ukitem(id, parent_id, hash) values (58, 0, modern.pkg_scr.gethash('154'));
    insert into modern.ukitem(id, parent_id, hash) values (59, 0, modern.pkg_scr.gethash('155'));
    insert into modern.ukitem(id, parent_id, hash) values (60, 0, modern.pkg_scr.gethash('156'));
    insert into modern.ukitem(id, parent_id, hash) values (62, 0, modern.pkg_scr.gethash('158'));
    insert into modern.ukitem(id, parent_id, hash) values (63, 0, modern.pkg_scr.gethash('159'));
    insert into modern.ukitem(id, parent_id, hash) values (64, 0, modern.pkg_scr.gethash('160'));
    insert into modern.ukitem(id, parent_id, hash) values (65, 0, modern.pkg_scr.gethash('161'));
    insert into modern.ukitem(id, parent_id, hash) values (66, 0, modern.pkg_scr.gethash('162'));
    insert into modern.ukitem(id, parent_id, hash) values (67, 0, modern.pkg_scr.gethash('163'));
    insert into modern.ukitem(id, parent_id, hash) values (68, 0, modern.pkg_scr.gethash('164'));
    insert into modern.ukitem(id, parent_id, hash) values (69, 0, modern.pkg_scr.gethash('165'));
    insert into modern.ukitem(id, parent_id, hash) values (71, 0, modern.pkg_scr.gethash('167'));
    insert into modern.ukitem(id, parent_id, hash) values (72, 0, modern.pkg_scr.gethash('168'));
    insert into modern.ukitem(id, parent_id, hash) values (73, 0, modern.pkg_scr.gethash('169'));
    insert into modern.ukitem(id, parent_id, hash) values (74, 0, modern.pkg_scr.gethash('170'));
    insert into modern.ukitem(id, parent_id, hash) values (75, 0, modern.pkg_scr.gethash('171'));
    insert into modern.ukitem(id, parent_id, hash) values (77, 0, modern.pkg_scr.gethash('172'));
    insert into modern.ukitem(id, parent_id, hash) values (78, 0, modern.pkg_scr.gethash('173'));
    insert into modern.ukitem(id, parent_id, hash) values (81, 0, modern.pkg_scr.gethash('175'));
    insert into modern.ukitem(id, parent_id, hash) values (82, 0, modern.pkg_scr.gethash('176'));
    insert into modern.ukitem(id, parent_id, hash) values (83, 0, modern.pkg_scr.gethash('177'));
    insert into modern.ukitem(id, parent_id, hash) values (84, 0, modern.pkg_scr.gethash('178'));
    insert into modern.ukitem(id, parent_id, hash) values (85, 0, modern.pkg_scr.gethash('179'));
    insert into modern.ukitem(id, parent_id, hash) values (86, 0, modern.pkg_scr.gethash('180'));
    insert into modern.ukitem(id, parent_id, hash) values (87, 0, modern.pkg_scr.gethash('181'));
    insert into modern.ukitem(id, parent_id, hash) values (91, 0, modern.pkg_scr.gethash('185'));
    insert into modern.ukitem(id, parent_id, hash) values (93, 0, modern.pkg_scr.gethash('186'));
    insert into modern.ukitem(id, parent_id, hash) values (95, 0, modern.pkg_scr.gethash('188'));
    insert into modern.ukitem(id, parent_id, hash) values (98, 0, modern.pkg_scr.gethash('191'));
    insert into modern.ukitem(id, parent_id, hash) values (99, 0, modern.pkg_scr.gethash('192'));
    insert into modern.ukitem(id, parent_id, hash) values (100, 0, modern.pkg_scr.gethash('193'));
    insert into modern.ukitem(id, parent_id, hash) values (101, 0, modern.pkg_scr.gethash('194'));
    insert into modern.ukitem(id, parent_id, hash) values (102, 0, modern.pkg_scr.gethash('195'));
    insert into modern.ukitem(id, parent_id, hash) values (103, 0, modern.pkg_scr.gethash('196'));
    insert into modern.ukitem(id, parent_id, hash) values (104, 0, modern.pkg_scr.gethash('197'));
    insert into modern.ukitem(id, parent_id, hash) values (105, 0, modern.pkg_scr.gethash('198'));
    insert into modern.ukitem(id, parent_id, hash) values (106, 0, modern.pkg_scr.gethash('199'));
    insert into modern.ukitem(id, parent_id, hash) values (107, 0, modern.pkg_scr.gethash('199.1'));
    insert into modern.ukitem(id, parent_id, hash) values (109, 0, modern.pkg_scr.gethash('201'));
    insert into modern.ukitem(id, parent_id, hash) values (110, 0, modern.pkg_scr.gethash('202'));
    insert into modern.ukitem(id, parent_id, hash) values (111, 0, modern.pkg_scr.gethash('203'));
    insert into modern.ukitem(id, parent_id, hash) values (112, 0, modern.pkg_scr.gethash('204'));
    insert into modern.ukitem(id, parent_id, hash) values (113, 0, modern.pkg_scr.gethash('205'));
    insert into modern.ukitem(id, parent_id, hash) values (114, 0, modern.pkg_scr.gethash('205.1'));
    insert into modern.ukitem(id, parent_id, hash) values (116, 0, modern.pkg_scr.gethash('206'));
    insert into modern.ukitem(id, parent_id, hash) values (117, 0, modern.pkg_scr.gethash('207'));
    insert into modern.ukitem(id, parent_id, hash) values (118, 0, modern.pkg_scr.gethash('208'));
    insert into modern.ukitem(id, parent_id, hash) values (119, 0, modern.pkg_scr.gethash('209'));
    insert into modern.ukitem(id, parent_id, hash) values (120, 0, modern.pkg_scr.gethash('210'));
    insert into modern.ukitem(id, parent_id, hash) values (121, 0, modern.pkg_scr.gethash('211'));
    insert into modern.ukitem(id, parent_id, hash) values (122, 0, modern.pkg_scr.gethash('212'));
    insert into modern.ukitem(id, parent_id, hash) values (123, 0, modern.pkg_scr.gethash('213'));
    insert into modern.ukitem(id, parent_id, hash) values (124, 0, modern.pkg_scr.gethash('214'));
    insert into modern.ukitem(id, parent_id, hash) values (125, 0, modern.pkg_scr.gethash('215'));
    insert into modern.ukitem(id, parent_id, hash) values (127, 0, modern.pkg_scr.gethash('215.2'));
    insert into modern.ukitem(id, parent_id, hash) values (128, 0, modern.pkg_scr.gethash('216'));
    insert into modern.ukitem(id, parent_id, hash) values (129, 0, modern.pkg_scr.gethash('217'));
    insert into modern.ukitem(id, parent_id, hash) values (131, 0, modern.pkg_scr.gethash('219'));
    insert into modern.ukitem(id, parent_id, hash) values (132, 0, modern.pkg_scr.gethash('220'));
    insert into modern.ukitem(id, parent_id, hash) values (133, 0, modern.pkg_scr.gethash('221'));
    insert into modern.ukitem(id, parent_id, hash) values (135, 0, modern.pkg_scr.gethash('223'));
    insert into modern.ukitem(id, parent_id, hash) values (136, 0, modern.pkg_scr.gethash('224'));
    insert into modern.ukitem(id, parent_id, hash) values (138, 0, modern.pkg_scr.gethash('226'));
    insert into modern.ukitem(id, parent_id, hash) values (139, 0, modern.pkg_scr.gethash('227'));
    insert into modern.ukitem(id, parent_id, hash) values (142, 0, modern.pkg_scr.gethash('228.2'));
    insert into modern.ukitem(id, parent_id, hash) values (143, 0, modern.pkg_scr.gethash('229'));
    insert into modern.ukitem(id, parent_id, hash) values (144, 0, modern.pkg_scr.gethash('230'));
    insert into modern.ukitem(id, parent_id, hash) values (148, 0, modern.pkg_scr.gethash('234'));
    insert into modern.ukitem(id, parent_id, hash) values (150, 0, modern.pkg_scr.gethash('236'));
    insert into modern.ukitem(id, parent_id, hash) values (153, 0, modern.pkg_scr.gethash('239'));
    insert into modern.ukitem(id, parent_id, hash) values (154, 0, modern.pkg_scr.gethash('240'));
    insert into modern.ukitem(id, parent_id, hash) values (155, 0, modern.pkg_scr.gethash('241'));
    insert into modern.ukitem(id, parent_id, hash) values (156, 0, modern.pkg_scr.gethash('242'));
    insert into modern.ukitem(id, parent_id, hash) values (158, 0, modern.pkg_scr.gethash('243'));
    insert into modern.ukitem(id, parent_id, hash) values (159, 0, modern.pkg_scr.gethash('244'));
    insert into modern.ukitem(id, parent_id, hash) values (160, 0, modern.pkg_scr.gethash('245'));
    insert into modern.ukitem(id, parent_id, hash) values (161, 0, modern.pkg_scr.gethash('246'));
    insert into modern.ukitem(id, parent_id, hash) values (163, 0, modern.pkg_scr.gethash('247'));
    insert into modern.ukitem(id, parent_id, hash) values (165, 0, modern.pkg_scr.gethash('249'));
    insert into modern.ukitem(id, parent_id, hash) values (166, 0, modern.pkg_scr.gethash('250'));
    insert into modern.ukitem(id, parent_id, hash) values (167, 0, modern.pkg_scr.gethash('251'));
    insert into modern.ukitem(id, parent_id, hash) values (168, 0, modern.pkg_scr.gethash('252'));
    insert into modern.ukitem(id, parent_id, hash) values (169, 0, modern.pkg_scr.gethash('253'));
    insert into modern.ukitem(id, parent_id, hash) values (170, 0, modern.pkg_scr.gethash('254'));
    insert into modern.ukitem(id, parent_id, hash) values (171, 0, modern.pkg_scr.gethash('255'));
    insert into modern.ukitem(id, parent_id, hash) values (172, 0, modern.pkg_scr.gethash('256'));
    insert into modern.ukitem(id, parent_id, hash) values (174, 0, modern.pkg_scr.gethash('257'));
    insert into modern.ukitem(id, parent_id, hash) values (175, 0, modern.pkg_scr.gethash('258'));
    insert into modern.ukitem(id, parent_id, hash) values (176, 0, modern.pkg_scr.gethash('259'));
    insert into modern.ukitem(id, parent_id, hash) values (177, 0, modern.pkg_scr.gethash('260'));
    insert into modern.ukitem(id, parent_id, hash) values (178, 0, modern.pkg_scr.gethash('261'));
    insert into modern.ukitem(id, parent_id, hash) values (179, 0, modern.pkg_scr.gethash('262'));
    insert into modern.ukitem(id, parent_id, hash) values (180, 0, modern.pkg_scr.gethash('263'));
    insert into modern.ukitem(id, parent_id, hash) values (181, 0, modern.pkg_scr.gethash('264'));
    insert into modern.ukitem(id, parent_id, hash) values (182, 0, modern.pkg_scr.gethash('266'));
    insert into modern.ukitem(id, parent_id, hash) values (183, 0, modern.pkg_scr.gethash('267'));
    insert into modern.ukitem(id, parent_id, hash) values (184, 0, modern.pkg_scr.gethash('268'));
    insert into modern.ukitem(id, parent_id, hash) values (186, 0, modern.pkg_scr.gethash('270'));
    insert into modern.ukitem(id, parent_id, hash) values (187, 0, modern.pkg_scr.gethash('271'));
    insert into modern.ukitem(id, parent_id, hash) values (188, 0, modern.pkg_scr.gethash('272'));
    insert into modern.ukitem(id, parent_id, hash) values (189, 0, modern.pkg_scr.gethash('273'));
    insert into modern.ukitem(id, parent_id, hash) values (190, 0, modern.pkg_scr.gethash('274'));
    insert into modern.ukitem(id, parent_id, hash) values (191, 0, modern.pkg_scr.gethash('275'));
    insert into modern.ukitem(id, parent_id, hash) values (192, 0, modern.pkg_scr.gethash('276'));
    insert into modern.ukitem(id, parent_id, hash) values (193, 0, modern.pkg_scr.gethash('277'));
    insert into modern.ukitem(id, parent_id, hash) values (194, 0, modern.pkg_scr.gethash('278'));
    insert into modern.ukitem(id, parent_id, hash) values (195, 0, modern.pkg_scr.gethash('279'));
    insert into modern.ukitem(id, parent_id, hash) values (196, 0, modern.pkg_scr.gethash('280'));
    insert into modern.ukitem(id, parent_id, hash) values (197, 0, modern.pkg_scr.gethash('281'));
    insert into modern.ukitem(id, parent_id, hash) values (198, 0, modern.pkg_scr.gethash('282'));
    insert into modern.ukitem(id, parent_id, hash) values (199, 0, modern.pkg_scr.gethash('282.1'));
    insert into modern.ukitem(id, parent_id, hash) values (200, 0, modern.pkg_scr.gethash('282.2'));
    insert into modern.ukitem(id, parent_id, hash) values (201, 0, modern.pkg_scr.gethash('283'));
    insert into modern.ukitem(id, parent_id, hash) values (202, 0, modern.pkg_scr.gethash('284'));
    insert into modern.ukitem(id, parent_id, hash) values (203, 0, modern.pkg_scr.gethash('285'));
    insert into modern.ukitem(id, parent_id, hash) values (204, 0, modern.pkg_scr.gethash('285.1'));
    insert into modern.ukitem(id, parent_id, hash) values (205, 0, modern.pkg_scr.gethash('285.2'));
    insert into modern.ukitem(id, parent_id, hash) values (206, 0, modern.pkg_scr.gethash('286'));
    insert into modern.ukitem(id, parent_id, hash) values (207, 0, modern.pkg_scr.gethash('287'));
    insert into modern.ukitem(id, parent_id, hash) values (208, 0, modern.pkg_scr.gethash('288'));
    insert into modern.ukitem(id, parent_id, hash) values (209, 0, modern.pkg_scr.gethash('289'));
    insert into modern.ukitem(id, parent_id, hash) values (210, 0, modern.pkg_scr.gethash('290'));
    insert into modern.ukitem(id, parent_id, hash) values (211, 0, modern.pkg_scr.gethash('291'));
    insert into modern.ukitem(id, parent_id, hash) values (212, 0, modern.pkg_scr.gethash('292'));
    insert into modern.ukitem(id, parent_id, hash) values (213, 0, modern.pkg_scr.gethash('293'));
    insert into modern.ukitem(id, parent_id, hash) values (214, 0, modern.pkg_scr.gethash('294'));
    insert into modern.ukitem(id, parent_id, hash) values (215, 0, modern.pkg_scr.gethash('295'));
    insert into modern.ukitem(id, parent_id, hash) values (217, 0, modern.pkg_scr.gethash('297'));
    insert into modern.ukitem(id, parent_id, hash) values (219, 0, modern.pkg_scr.gethash('299'));
    insert into modern.ukitem(id, parent_id, hash) values (220, 0, modern.pkg_scr.gethash('300'));
    insert into modern.ukitem(id, parent_id, hash) values (221, 0, modern.pkg_scr.gethash('301'));
    insert into modern.ukitem(id, parent_id, hash) values (222, 0, modern.pkg_scr.gethash('302'));
    insert into modern.ukitem(id, parent_id, hash) values (223, 0, modern.pkg_scr.gethash('303'));
    insert into modern.ukitem(id, parent_id, hash) values (224, 0, modern.pkg_scr.gethash('304'));
    insert into modern.ukitem(id, parent_id, hash) values (225, 0, modern.pkg_scr.gethash('305'));
    insert into modern.ukitem(id, parent_id, hash) values (226, 0, modern.pkg_scr.gethash('306'));
    insert into modern.ukitem(id, parent_id, hash) values (227, 0, modern.pkg_scr.gethash('307'));
    insert into modern.ukitem(id, parent_id, hash) values (228, 0, modern.pkg_scr.gethash('308'));
    insert into modern.ukitem(id, parent_id, hash) values (230, 0, modern.pkg_scr.gethash('310'));
    insert into modern.ukitem(id, parent_id, hash) values (231, 0, modern.pkg_scr.gethash('311'));
    insert into modern.ukitem(id, parent_id, hash) values (233, 0, modern.pkg_scr.gethash('313'));
    insert into modern.ukitem(id, parent_id, hash) values (234, 0, modern.pkg_scr.gethash('314'));
    insert into modern.ukitem(id, parent_id, hash) values (235, 0, modern.pkg_scr.gethash('315'));
    insert into modern.ukitem(id, parent_id, hash) values (236, 0, modern.pkg_scr.gethash('316'));
    insert into modern.ukitem(id, parent_id, hash) values (237, 0, modern.pkg_scr.gethash('317'));
    insert into modern.ukitem(id, parent_id, hash) values (238, 0, modern.pkg_scr.gethash('318'));
    insert into modern.ukitem(id, parent_id, hash) values (239, 0, modern.pkg_scr.gethash('319'));
    insert into modern.ukitem(id, parent_id, hash) values (241, 0, modern.pkg_scr.gethash('321'));
    insert into modern.ukitem(id, parent_id, hash) values (242, 0, modern.pkg_scr.gethash('322'));
    insert into modern.ukitem(id, parent_id, hash) values (243, 0, modern.pkg_scr.gethash('322.1'));
    insert into modern.ukitem(id, parent_id, hash) values (244, 0, modern.pkg_scr.gethash('323'));
    insert into modern.ukitem(id, parent_id, hash) values (245, 0, modern.pkg_scr.gethash('324'));
    insert into modern.ukitem(id, parent_id, hash) values (247, 0, modern.pkg_scr.gethash('326'));
    insert into modern.ukitem(id, parent_id, hash) values (248, 0, modern.pkg_scr.gethash('327'));
    insert into modern.ukitem(id, parent_id, hash) values (250, 0, modern.pkg_scr.gethash('328'));
    insert into modern.ukitem(id, parent_id, hash) values (251, 0, modern.pkg_scr.gethash('329'));
    insert into modern.ukitem(id, parent_id, hash) values (252, 0, modern.pkg_scr.gethash('330'));
    insert into modern.ukitem(id, parent_id, hash) values (253, 0, modern.pkg_scr.gethash('332'));
    insert into modern.ukitem(id, parent_id, hash) values (254, 0, modern.pkg_scr.gethash('333'));
    insert into modern.ukitem(id, parent_id, hash) values (255, 0, modern.pkg_scr.gethash('334'));
    insert into modern.ukitem(id, parent_id, hash) values (257, 0, modern.pkg_scr.gethash('336'));
    insert into modern.ukitem(id, parent_id, hash) values (258, 0, modern.pkg_scr.gethash('337'));
    insert into modern.ukitem(id, parent_id, hash) values (259, 0, modern.pkg_scr.gethash('338'));
    insert into modern.ukitem(id, parent_id, hash) values (261, 0, modern.pkg_scr.gethash('340'));
    insert into modern.ukitem(id, parent_id, hash) values (262, 0, modern.pkg_scr.gethash('341'));
    insert into modern.ukitem(id, parent_id, hash) values (263, 0, modern.pkg_scr.gethash('342'));
    insert into modern.ukitem(id, parent_id, hash) values (265, 0, modern.pkg_scr.gethash('344'));
    insert into modern.ukitem(id, parent_id, hash) values (266, 0, modern.pkg_scr.gethash('345'));
    insert into modern.ukitem(id, parent_id, hash) values (267, 0, modern.pkg_scr.gethash('346'));
    insert into modern.ukitem(id, parent_id, hash) values (268, 0, modern.pkg_scr.gethash('347'));
    insert into modern.ukitem(id, parent_id, hash) values (269, 0, modern.pkg_scr.gethash('348'));
    insert into modern.ukitem(id, parent_id, hash) values (271, 0, modern.pkg_scr.gethash('350'));
    insert into modern.ukitem(id, parent_id, hash) values (272, 0, modern.pkg_scr.gethash('351'));
    insert into modern.ukitem(id, parent_id, hash) values (273, 0, modern.pkg_scr.gethash('352'));
    insert into modern.ukitem(id, parent_id, hash) values (274, 0, modern.pkg_scr.gethash('353'));
    insert into modern.ukitem(id, parent_id, hash) values (275, 0, modern.pkg_scr.gethash('354'));
    insert into modern.ukitem(id, parent_id, hash) values (276, 0, modern.pkg_scr.gethash('355'));
    insert into modern.ukitem(id, parent_id, hash) values (277, 0, modern.pkg_scr.gethash('356'));
    insert into modern.ukitem(id, parent_id, hash) values (278, 0, modern.pkg_scr.gethash('357'));
    insert into modern.ukitem(id, parent_id, hash) values (279, 0, modern.pkg_scr.gethash('358'));
    insert into modern.ukitem(id, parent_id, hash) values (280, 0, modern.pkg_scr.gethash('359'));
    insert into modern.ukitem(id, parent_id, hash) values (281, 0, modern.pkg_scr.gethash('360'));

    insert into modern.ukarticl(id, artcl,note) values (1, '105', 'Убийство');
    insert into modern.ukarticl(id, artcl,note) values (2, '106', 'Убийство матерью новорожденного ребенка');
    insert into modern.ukarticl(id, artcl,note) values (3, '107', 'Убийство, совершенное в состоянии аффекта');
    insert into modern.ukarticl(id, artcl,note) values (5, '109', 'Причинение смерти по неосторожности');
    insert into modern.ukarticl(id, artcl,note) values (6, '110', 'Доведение до самоубийства');
    insert into modern.ukarticl(id, artcl,note) values (7, '111', 'Умышленное причинение тяжкого вреда здоровью');
    insert into modern.ukarticl(id, artcl,note) values (8, '112', 'Умышленное причинение средней тяжести вреда здоровью');
    insert into modern.ukarticl(id, artcl,note) values (10, '113', 'Причинение тяжкого или средней тяжести вреда здоровью в состоянии аффекта');
    insert into modern.ukarticl(id, artcl,note) values (12, '115', 'Умышленное причинение легкого вреда здоровью');
    insert into modern.ukarticl(id, artcl,note) values (13, '116', 'Побои');
    insert into modern.ukarticl(id, artcl,note) values (14, '117', 'Истязание');
    insert into modern.ukarticl(id, artcl,note) values (16, '118', 'Причинение тяжкого вреда здоровью по неосторожности');
    insert into modern.ukarticl(id, artcl,note) values (17, '119', 'Угроза убийством или причинением вреда');
    insert into modern.ukarticl(id, artcl,note) values (18, '120', 'Принуждение к изъятию органов или тканей человека для трансплантации');
    insert into modern.ukarticl(id, artcl,note) values (19, '121', 'Заражение венерической болезнью');
    insert into modern.ukarticl(id, artcl,note) values (20, '122', 'Заражение ВИЧ-инфекцией');
    insert into modern.ukarticl(id, artcl,note) values (21, '123', 'Незаконное производство аборта');
    insert into modern.ukarticl(id, artcl,note) values (22, '124', 'Неоказание помощи больному');
    insert into modern.ukarticl(id, artcl,note) values (23, '125', 'Оставление в опасности');
    insert into modern.ukarticl(id, artcl,note) values (24, '126', 'Похищение человеа');
    insert into modern.ukarticl(id, artcl,note) values (25, '127', 'Незаконное лишение свободы');
    insert into modern.ukarticl(id, artcl,note) values (26, '127.1', 'Торговля людьми');
    insert into modern.ukarticl(id, artcl,note) values (27, '127.2', 'Использование рабского труда');
    insert into modern.ukarticl(id, artcl,note) values (28, '128', 'Незаконное помещение в психоатрический стационар');
    insert into modern.ukarticl(id, artcl,note) values (30, '129', 'Клевета');
    insert into modern.ukarticl(id, artcl,note) values (31, '130', 'Оскорбление');
    insert into modern.ukarticl(id, artcl,note) values (32, '131', 'Изнасилование');
    insert into modern.ukarticl(id, artcl,note) values (33, '132', 'Насильственные действия сексуального характера');
    insert into modern.ukarticl(id, artcl,note) values (34, '133', 'Понуждение к действиям сексуального характера');
    insert into modern.ukarticl(id, artcl,note) values (35, '134', 'Половое сношение и иные действия сексуального характера');
    insert into modern.ukarticl(id, artcl,note) values (36, '135', 'Развратные действия');
    insert into modern.ukarticl(id, artcl,note) values (37, '136', 'Нарушение равенства прав и свобод человека и гражданина');
    insert into modern.ukarticl(id, artcl,note) values (38, '137', 'Нарушение неприкосновенности чатной жизни');
    insert into modern.ukarticl(id, artcl,note) values (40, '139', 'Нарушение неприкосновенности жилища');
    insert into modern.ukarticl(id, artcl,note) values (41, '140', 'Отказ в предоставлении гражданину информации');
    insert into modern.ukarticl(id, artcl,note) values (43, '141.1', 'Наруш. порядка финансирования избир. кампании');
    insert into modern.ukarticl(id, artcl,note) values (44, '142', 'Фальсификация избирательных документов, документов референдума');
    insert into modern.ukarticl(id, artcl,note) values (45, '142.1', 'Фальсификация итогов голосования');
    insert into modern.ukarticl(id, artcl,note) values (46, '143', 'Нарушение правил охраны труда');
    insert into modern.ukarticl(id, artcl,note) values (47, '144', 'Воспрепятствование законной проф. деятельности журналистов');
    insert into modern.ukarticl(id, artcl,note) values (49, '145.1', 'Невыплата заработной платы, пенсий, стипендий, пособий и иных выплат');
    insert into modern.ukarticl(id, artcl,note) values (50, '146', 'Нарушение авторских и смежных прав');
    insert into modern.ukarticl(id, artcl,note) values (51, '147', 'Нарушение изобретательских и патентных прав');
    insert into modern.ukarticl(id, artcl,note) values (52, '148', 'Воспрепятствование осуществлению права на свободу совести и вероисповеданий ');
    insert into modern.ukarticl(id, artcl,note) values (54, '150', 'Вовлечение несовершеннолетнего в совершение преступления ');
    insert into modern.ukarticl(id, artcl,note) values (56, '151', 'Вовлечение несовершеннолетнего в совершение антиобщественных действий');
    insert into modern.ukarticl(id, artcl,note) values (57, '153', 'Подмена ребенка');
    insert into modern.ukarticl(id, artcl,note) values (58, '154', 'Незаконное усыновление (удочерение)');
    insert into modern.ukarticl(id, artcl,note) values (59, '155', 'Разглашение тайны усыновления (удочерения)');
    insert into modern.ukarticl(id, artcl,note) values (60, '156', 'Неисполнение обязаностей по воспитанию несовершеннолетнего');
    insert into modern.ukarticl(id, artcl,note) values (62, '158', 'Кража');
    insert into modern.ukarticl(id, artcl,note) values (63, '159', 'Мошенничество');
    insert into modern.ukarticl(id, artcl,note) values (64, '160', 'Присвоение и растрата');
    insert into modern.ukarticl(id, artcl,note) values (65, '161', 'Грабеж');
    insert into modern.ukarticl(id, artcl,note) values (66, '162', 'Разбой');
    insert into modern.ukarticl(id, artcl,note) values (67, '163', 'Вымогательство');
    insert into modern.ukarticl(id, artcl,note) values (68, '164', 'Хищение предметов, имеющих особую ценность');
    insert into modern.ukarticl(id, artcl,note) values (69, '165', 'Причинение имущественного ущерба путем обмана или злоупотребления доверием');
    insert into modern.ukarticl(id, artcl,note) values (71, '167', 'Умышленные уничтожение или повреждение имущества');
    insert into modern.ukarticl(id, artcl,note) values (72, '168', 'Уничтожение или повреждение имущества по неосторожности');
    insert into modern.ukarticl(id, artcl,note) values (73, '169', 'Воспрепятствование законной предпринимательской или иной деятельности');
    insert into modern.ukarticl(id, artcl,note) values (74, '170', 'Регистрация законных сделок с землей');
    insert into modern.ukarticl(id, artcl,note) values (75, '171', 'Незаконное предпринимательство');
    insert into modern.ukarticl(id, artcl,note) values (77, '172', 'Незаконная банковская деятельность');
    insert into modern.ukarticl(id, artcl,note) values (78, '173', 'Лжепредпринимательство');
    insert into modern.ukarticl(id, artcl,note) values (81, '175', 'Приобретение или сбыт имущества, заведомо добытого преступным путем');
    insert into modern.ukarticl(id, artcl,note) values (82, '176', 'Незаконное получение кредита');
    insert into modern.ukarticl(id, artcl,note) values (83, '177', 'Злостное уклонение от погашения кредиторской задолженности');
    insert into modern.ukarticl(id, artcl,note) values (84, '178', 'Недопущение, ограничение или устранение конкуренции');
    insert into modern.ukarticl(id, artcl,note) values (85, '179', 'Принуждение к совершению сделки или к отказу от ее совершения ');
    insert into modern.ukarticl(id, artcl,note) values (86, '180', 'Незаконное использование товарного знака');
    insert into modern.ukarticl(id, artcl,note) values (87, '181', 'Нарушение правил изготовления и использования государственных пробирных клейм');
    insert into modern.ukarticl(id, artcl,note) values (91, '185', 'Злоупотребление при эмиссии ценных бумаг');
    insert into modern.ukarticl(id, artcl,note) values (93, '186', 'Изготовление или сбыт поддельных денег или ценных бумаг');
    insert into modern.ukarticl(id, artcl,note) values (95, '188', 'Контрабанда');
    insert into modern.ukarticl(id, artcl,note) values (98, '191', 'Незаконный оборот драг. металлов, природных камней или жемчуга');
    insert into modern.ukarticl(id, artcl,note) values (99, '192', 'Нарушение правил сдачи государству драг. металлов и драг. комней');
    insert into modern.ukarticl(id, artcl,note) values (100, '193', 'Невозвращение из-за границы средств в иностранной валюте');
    insert into modern.ukarticl(id, artcl,note) values (101, '194', 'Уклонение от уплаты таможенных платежей, взым. с организации или физ. лица');
    insert into modern.ukarticl(id, artcl,note) values (102, '195', 'Неправомерные действия при банкротстве');
    insert into modern.ukarticl(id, artcl,note) values (103, '196', 'Преднамеренное банкротство');
    insert into modern.ukarticl(id, artcl,note) values (104, '197', 'Фиктивное банкротство');
    insert into modern.ukarticl(id, artcl,note) values (105, '198', 'Уклонение от уплаты налогов и (или) сборов с физического лица');
    insert into modern.ukarticl(id, artcl,note) values (106, '199', 'Уклонение от уплаты налогов и (или) сборов с организации');
    insert into modern.ukarticl(id, artcl,note) values (107, '199.1', 'Неисполнение обязанностей налогового агента');
    insert into modern.ukarticl(id, artcl,note) values (109, '201', 'Злоупотребление полномочиями');
    insert into modern.ukarticl(id, artcl,note) values (110, '202', 'Злоупотребление правомочиями частными нотариусами и аудиторами');
    insert into modern.ukarticl(id, artcl,note) values (111, '203', 'Превышение полномочий служащими частных охранных или детективных служб');
    insert into modern.ukarticl(id, artcl,note) values (112, '204', 'Коммерческий подкуп');
    insert into modern.ukarticl(id, artcl,note) values (113, '205', 'Террористический акт');
    insert into modern.ukarticl(id, artcl,note) values (114, '205.1', 'Содействие террористической деятельности');
    insert into modern.ukarticl(id, artcl,note) values (116, '206', 'Захват заложника');
    insert into modern.ukarticl(id, artcl,note) values (117, '207', 'Заведомо ложное сообщение об акте терроризма');
    insert into modern.ukarticl(id, artcl,note) values (118, '208', 'Организация незаконного вооруженного формирования или участие в нем');
    insert into modern.ukarticl(id, artcl,note) values (119, '209', 'Бандитизм');
    insert into modern.ukarticl(id, artcl,note) values (120, '210', 'Организация преступного сообщества (преступной организации)');
    insert into modern.ukarticl(id, artcl,note) values (121, '211', 'Угон судна воздушного или водного транспорта либо ж/д подвижного состава');
    insert into modern.ukarticl(id, artcl,note) values (122, '212', 'Массовые беспорядки ');
    insert into modern.ukarticl(id, artcl,note) values (123, '213', 'Хулиганство');
    insert into modern.ukarticl(id, artcl,note) values (124, '214', 'Вандализм');
    insert into modern.ukarticl(id, artcl,note) values (125, '215', 'Нарушение правил безопасности на объектах атомной энергетики');
    insert into modern.ukarticl(id, artcl,note) values (127, '215.2', 'Приведение в негодность объектов жизнеобеспечения');
    insert into modern.ukarticl(id, artcl,note) values (128, '216', 'Нарушение правил безопасности при проведении горных, строительных или иных работ');
    insert into modern.ukarticl(id, artcl,note) values (129, '217', 'Нарушение правил безопасности на взрывоопасных объектах');
    insert into modern.ukarticl(id, artcl,note) values (131, '219', 'Нарушение правил пожарной безопасности');
    insert into modern.ukarticl(id, artcl,note) values (132, '220', 'Незаконное обращение с ядерными материалами или радиоактивными веществами');
    insert into modern.ukarticl(id, artcl,note) values (133, '221', 'Хищение либо вымогательство ядерных материалов или радиоактивных веществ');
    insert into modern.ukarticl(id, artcl,note) values (135, '223', 'Незаконное изготовление оружия');
    insert into modern.ukarticl(id, artcl,note) values (136, '224', 'Небрежное хранение огнестрельного оружия');
    insert into modern.ukarticl(id, artcl,note) values (138, '226', 'Хищение либо вымогательство оружия, боеприпасов, взрыв. вещ-в и устройств');
    insert into modern.ukarticl(id, artcl,note) values (139, '227', 'Пиратство');
    insert into modern.ukarticl(id, artcl,note) values (142, '228.2', 'Нарушение правил оборота наркотических средств или психотропных веществ');
    insert into modern.ukarticl(id, artcl,note) values (143, '229', 'Хищение либо вымогательствонаркотических средств или психотропных веществ');
    insert into modern.ukarticl(id, artcl,note) values (144, '230', 'Склонение к потреблению наркотических средств или психотропных веществ');
    insert into modern.ukarticl(id, artcl,note) values (148, '234', 'Незаконный оборот сильнодействующих или ядовитых веществ в целях сбыта');
    insert into modern.ukarticl(id, artcl,note) values (150, '236', 'Нарушение  санитарно-эпидемиологических правил');
    insert into modern.ukarticl(id, artcl,note) values (153, '239', 'Организация объединения, посягающего на личность и права граждан');
    insert into modern.ukarticl(id, artcl,note) values (154, '240', 'Вовлечение в занитие проституцией');
    insert into modern.ukarticl(id, artcl,note) values (155, '241', 'Организация занития проституцией');
    insert into modern.ukarticl(id, artcl,note) values (156, '242', 'Незаконное распространение порнографических материалов и предметов');
    insert into modern.ukarticl(id, artcl,note) values (158, '243', 'Уничтожение или повреждение памятников истории и культуры');
    insert into modern.ukarticl(id, artcl,note) values (159, '244', 'Надругательство над телами умерших и местами их захоронения');
    insert into modern.ukarticl(id, artcl,note) values (160, '245', 'Жестокое обращение с животными');
    insert into modern.ukarticl(id, artcl,note) values (161, '246', 'Нарушение правил охраны окружающей средыпри пр-ве работ');
    insert into modern.ukarticl(id, artcl,note) values (163, '247', 'Нарушение правил обращения экологически опасных веществ и отходов');
    insert into modern.ukarticl(id, artcl,note) values (165, '249', 'Нарушение вет. правил и правил, уст. для борьбы с болезнями и вредителями растений');
    insert into modern.ukarticl(id, artcl,note) values (166, '250', 'Загрязнение вод');
    insert into modern.ukarticl(id, artcl,note) values (167, '251', 'Загрязнение атмосферы');
    insert into modern.ukarticl(id, artcl,note) values (168, '252', 'Загрязнение морской среды');
    insert into modern.ukarticl(id, artcl,note) values (169, '253', 'Нарушение зак-ва РФ о континентальном шельфе и об искл. эконом. зоне РФ');
    insert into modern.ukarticl(id, artcl,note) values (170, '254', 'Порча земли');
    insert into modern.ukarticl(id, artcl,note) values (171, '255', 'Нарушение правил охраны и испол. недр');
    insert into modern.ukarticl(id, artcl,note) values (172, '256', 'Незаконная добыча водных животных и растений');
    insert into modern.ukarticl(id, artcl,note) values (174, '257', 'Нарушение правил охраны рыбных запасов');
    insert into modern.ukarticl(id, artcl,note) values (175, '258', 'Незаконная охота');
    insert into modern.ukarticl(id, artcl,note) values (176, '259', 'Уничтожение критич. местообитаний для организмов, занес. в Красную книгу РФ');
    insert into modern.ukarticl(id, artcl,note) values (177, '260', 'Незаконная порубка деревьев и кустарников');
    insert into modern.ukarticl(id, artcl,note) values (178, '261', 'Уничтожение или повреждение лесов');
    insert into modern.ukarticl(id, artcl,note) values (179, '262', 'Нарушение режима особоохраняемых природ. тер-рий и объектов');
    insert into modern.ukarticl(id, artcl,note) values (180, '263', 'Наруш. правил безопасности движения и эксплуатиции ж/д, воздушного или водного ');
    insert into modern.ukarticl(id, artcl,note) values (181, '264', 'Нарушение ПДД и эксплуатации трансп. средств');
    insert into modern.ukarticl(id, artcl,note) values (182, '266', 'Недоброкач. ремонт трансп. средств и выпуск их в экспл-цию с тех. неисправностями');
    insert into modern.ukarticl(id, artcl,note) values (183, '267', 'Приведение в негодность трансп. средств или путей сообщения');
    insert into modern.ukarticl(id, artcl,note) values (184, '268', 'Нарушение правил, обеспеч. безопасную работу транспорта');
    insert into modern.ukarticl(id, artcl,note) values (186, '270', 'Неоказание капитаном судна помощи терпящим бедствие ');
    insert into modern.ukarticl(id, artcl,note) values (187, '271', 'Нарушение правил международных полетов');
    insert into modern.ukarticl(id, artcl,note) values (188, '272', 'Неправомерный доступ к компьютерной информации');
    insert into modern.ukarticl(id, artcl,note) values (189, '273', 'Создание, использование и распр. вредоносных программ для ЭВМ ');
    insert into modern.ukarticl(id, artcl,note) values (190, '274', 'Нарушение правил эксплуатации ЭВМ, системы ЭВМ или их сети');
    insert into modern.ukarticl(id, artcl,note) values (191, '275', 'Государственная измена');
    insert into modern.ukarticl(id, artcl,note) values (192, '276', 'Шпионаж');
    insert into modern.ukarticl(id, artcl,note) values (193, '277', 'Посягательство на жизнь гос. или общ. деятеля');
    insert into modern.ukarticl(id, artcl,note) values (194, '278', 'Насильственный захват власти или насил. удержание власти');
    insert into modern.ukarticl(id, artcl,note) values (195, '279', 'Вооруженный мятеж');
    insert into modern.ukarticl(id, artcl,note) values (196, '280', 'Публичные призывы к осуществлению экстремистской деятельности');
    insert into modern.ukarticl(id, artcl,note) values (197, '281', 'Диверсия');
    insert into modern.ukarticl(id, artcl,note) values (198, '282', 'Возбуждение ненависти либо вражды, а равно унижение чел. достоинства ');
    insert into modern.ukarticl(id, artcl,note) values (199, '282.1', 'Организация экстремистского сообщества');
    insert into modern.ukarticl(id, artcl,note) values (200, '282.2', 'Организация деят-ти экстремистской орг-ции');
    insert into modern.ukarticl(id, artcl,note) values (201, '283', 'Разглашение гос. тайны');
    insert into modern.ukarticl(id, artcl,note) values (202, '284', 'Утрата документов, содерж. гос. тайну');
    insert into modern.ukarticl(id, artcl,note) values (203, '285', 'Злоупотребление должностными полномочиями');
    insert into modern.ukarticl(id, artcl,note) values (204, '285.1', 'Нецелевое расходование бюджетных средств');
    insert into modern.ukarticl(id, artcl,note) values (205, '285.2', 'Нецелевое расходование ср-в гос. внебюджетных фондов');
    insert into modern.ukarticl(id, artcl,note) values (206, '286', 'Превышение должностных полномочий');
    insert into modern.ukarticl(id, artcl,note) values (207, '287', 'Отказ в предост. инф-ции Фед. Собранию РФ или Счетной палате РФ');
    insert into modern.ukarticl(id, artcl,note) values (208, '288', 'Присвоение полномочий должностного лица');
    insert into modern.ukarticl(id, artcl,note) values (209, '289', 'Незаконное участие в предпринимательской  деят-ти');
    insert into modern.ukarticl(id, artcl,note) values (210, '290', 'Получение взятки');
    insert into modern.ukarticl(id, artcl,note) values (211, '291', 'Дача взятки');
    insert into modern.ukarticl(id, artcl,note) values (212, '292', 'Служебный подлог');
    insert into modern.ukarticl(id, artcl,note) values (213, '293', 'Халатность');
    insert into modern.ukarticl(id, artcl,note) values (214, '294', 'Воспрепят-ние осущ-нию правосудия и пр-ву предварит. расследования');
    insert into modern.ukarticl(id, artcl,note) values (215, '295', 'Посягательство на жизнь лица, осущ-го правосудие или предварит. расследование');
    insert into modern.ukarticl(id, artcl,note) values (217, '297', 'Неуважение к суду');
    insert into modern.ukarticl(id, artcl,note) values (219, '299', 'Привлечение заведомо невиновного к угол. отв-ти');
    insert into modern.ukarticl(id, artcl,note) values (220, '300', 'Незаконное освобождение от угол. отв-ти');
    insert into modern.ukarticl(id, artcl,note) values (221, '301', 'Незаконные задержание, заключ. под стражу или содержание под стражей ');
    insert into modern.ukarticl(id, artcl,note) values (222, '302', 'Принуждение к даче показаний');
    insert into modern.ukarticl(id, artcl,note) values (223, '303', 'Фальсификация доказательств');
    insert into modern.ukarticl(id, artcl,note) values (224, '304', 'Провокация взятки либо коммерческого подкупа');
    insert into modern.ukarticl(id, artcl,note) values (225, '305', 'Вынесение заведомо неправосудных приговора, решения либо иного суд. акта');
    insert into modern.ukarticl(id, artcl,note) values (226, '306', 'Заведомо ложный донос');
    insert into modern.ukarticl(id, artcl,note) values (227, '307', 'Заведомо ложные показания, заключ-ние эксперта, специалиста или неправ. перевод');
    insert into modern.ukarticl(id, artcl,note) values (228, '308', 'Отказ свидетеля или потерпевшего от дачи показаний');
    insert into modern.ukarticl(id, artcl,note) values (230, '310', 'Разглашение данных предварительного расследования');
    insert into modern.ukarticl(id, artcl,note) values (231, '311', 'Разглашение сведений о мерах безоп-ти, прим. в отн-ии судьи и уч-ков угол. процесса ');
    insert into modern.ukarticl(id, artcl,note) values (233, '313', 'Побег из места лишения свободы, из-под ареста или из-под стражи');
    insert into modern.ukarticl(id, artcl,note) values (234, '314', 'Уклонение от отбывания лишения свободы');
    insert into modern.ukarticl(id, artcl,note) values (235, '315', 'Неисполнение приговора суда, решения суда или иного суд. акта');
    insert into modern.ukarticl(id, artcl,note) values (236, '316', 'Укрывательство преступлений');
    insert into modern.ukarticl(id, artcl,note) values (237, '317', 'Посягательство на жизнь сотрудника правоохран. органа');
    insert into modern.ukarticl(id, artcl,note) values (238, '318', 'Применение насилия в отн-ии представителя власти ');
    insert into modern.ukarticl(id, artcl,note) values (239, '319', 'Оскорбление предст-ля власти');
    insert into modern.ukarticl(id, artcl,note) values (241, '321', 'Дезорганизация деят-ти учреждений, обеспеч. изоляцию от общ-ва');
    insert into modern.ukarticl(id, artcl,note) values (242, '322', 'Незаконное пересечение Гос. границы РФ');
    insert into modern.ukarticl(id, artcl,note) values (243, '322.1', 'Организация незаконной миграции');
    insert into modern.ukarticl(id, artcl,note) values (244, '323', 'Противоправное изменение Гос. границы РФ');
    insert into modern.ukarticl(id, artcl,note) values (245, '324', 'Приобретение или сбыт офиц. док-тов и гос. наград');
    insert into modern.ukarticl(id, artcl,note) values (247, '326', 'Подделка или уничтожение идентификационного номера трансп. средства');
    insert into modern.ukarticl(id, artcl,note) values (248, '327', 'Подделка, изгот. или сбыт поддельных док-тов, гос. наград, штампов, печатей, бланков');
    insert into modern.ukarticl(id, artcl,note) values (250, '328', 'Уклонениие от прохождения военной и альтернативной гражданской службы');
    insert into modern.ukarticl(id, artcl,note) values (251, '329', 'Надругательство над Гос. гербом РФ или Гос. флагом РФ');
    insert into modern.ukarticl(id, artcl,note) values (252, '330', 'Самоуправство');
    insert into modern.ukarticl(id, artcl,note) values (253, '332', 'Неисполнение приказа');
    insert into modern.ukarticl(id, artcl,note) values (254, '333', 'Сопротивление начальнику или принужд. его к нарушению обязан-тей военной службы');
    insert into modern.ukarticl(id, artcl,note) values (255, '334', 'Насильственные действия в отн-и начальника');
    insert into modern.ukarticl(id, artcl,note) values (257, '336', 'Оскорбление военнослужащего');
    insert into modern.ukarticl(id, artcl,note) values (258, '337', 'Самовольное оставление части или места службы');
    insert into modern.ukarticl(id, artcl,note) values (259, '338', 'Дезертирство');
    insert into modern.ukarticl(id, artcl,note) values (261, '340', 'Нарушение правил несения боевого дежурства');
    insert into modern.ukarticl(id, artcl,note) values (262, '341', 'Нарушение правил несения пограничной службы');
    insert into modern.ukarticl(id, artcl,note) values (263, '342', 'Нарушение уставных правил караульной службы');
    insert into modern.ukarticl(id, artcl,note) values (265, '344', 'Нарушение уставных правил несения внутр. службы и патрул-я в гарнизоне');
    insert into modern.ukarticl(id, artcl,note) values (266, '345', 'Оставление погибающего военного корабля');
    insert into modern.ukarticl(id, artcl,note) values (267, '346', 'Умыш. уничтожение или повреждение воен. им-ва');
    insert into modern.ukarticl(id, artcl,note) values (268, '347', 'Уничтожение или повреждение воен. им-ва по неосторожности');
    insert into modern.ukarticl(id, artcl,note) values (269, '348', 'Утрата военного им-ва');
    insert into modern.ukarticl(id, artcl,note) values (271, '350', 'Нарушение правил вождения и эксплуатации машин');
    insert into modern.ukarticl(id, artcl,note) values (272, '351', 'Нарушение правил полетов или подготовки к ним');
    insert into modern.ukarticl(id, artcl,note) values (273, '352', 'Нарушение правил кораблевождения');
    insert into modern.ukarticl(id, artcl,note) values (274, '353', 'Планирование, подготовка, развязывание или ведение агрессивной войны');
    insert into modern.ukarticl(id, artcl,note) values (275, '354', 'Публичные призывы к развязыванию агрессивной войны');
    insert into modern.ukarticl(id, artcl,note) values (276, '355', 'Разработка, пр-во, накопление, приобретение или сбыт оружия массового поражения');
    insert into modern.ukarticl(id, artcl,note) values (277, '356', 'Применение запрещенных средств и методов ведения войны');
    insert into modern.ukarticl(id, artcl,note) values (278, '357', 'Геноцид');
    insert into modern.ukarticl(id, artcl,note) values (279, '358', 'Экоцид');
    insert into modern.ukarticl(id, artcl,note) values (280, '359', 'Наемничество');
    insert into modern.ukarticl(id, artcl,note) values (281, '360', 'Нападение на лиц или учреждения, которые пользуются международной защитой');

    -- статья 18
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 0, modern.pkg_scr.getHash('18'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '18',
      'Рецидив преступлений');
    select modern.ukitem_seq.currval into curr_state_val from dual;
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('18 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Рецидивом преступлений признается совершение умышленного преступления лицом, имеющим судимость за ранее совершенное умышленное преступление');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('18 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Рецидив преступлений признается опасным');
      select modern.ukitem_seq.currval into curr_past_val from dual;
        -- пункт а
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'а',
        'Рецидивом преступлений признается совершение умышленного преступления лицом, имеющим судимость за ранее совершенное умышленное преступление');
        -- пункт б
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
        'при совершении лицом тяжкого преступления, если ранее оно было осуждено за тяжкое или особо тяжкое преступление к реальному лишению свободы');
        -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('18 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Рецидив преступлений признается особо опасным');
      select modern.ukitem_seq.currval into curr_past_val from dual;
        -- пункт а
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'а',
        'при совершении лицом тяжкого преступления, за которое оно осуждается к реальному лишению свободы, если ранее это лицо два раза было осуждено за тяжкое преступление к реальному лишению свободы');
        -- пункт б
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
        'при совершении лицом особо тяжкого преступления, если ранее оно два раза было осуждено за тяжкое преступление или ранее осуждалось за особо тяжкое преступление');
        -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('18 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'При признании рецидива преступлений не учитываются');
      select modern.ukitem_seq.currval into curr_past_val from dual;
        -- пункт а
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 4 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'а',
        'судимости за умышленные преступления небольшой тяжести');
        -- пункт б
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 4 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
        'судимости за преступления, совершенные лицом в возрасте до восемнадцати лет');
        -- пункт в
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('18 4 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
        'судимости за преступления, осуждение за которые признавалось условным либо по которым предоставлялась отсрочка исполнения приговора, если условное осуждение или отсрочка исполнения приговора не отменялись и лицо не направлялось для отбывания наказания в места лишения свободы, а также судимости, снятые или погашенные в порядке, установленном статьей 86 настоящего Кодекса');
        -- часть 5
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('18 5'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '5',
        'Рецидив преступлений влечет более строгое наказание на основании и в пределах, предусмотренных настоящим Кодексом');

    -- статья 30
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 0, modern.pkg_scr.getHash('30'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '30',
      'Приготовление к преступлению и покушение на преступление');
    select modern.ukitem_seq.currval into curr_state_val from dual;
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('30 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Приготовлением к преступлению признаются приискание, изготовление или приспособление лицом средств или орудий совершения преступления, приискание соучастников преступления, сговор на совершение преступления либо иное умышленное создание условий для совершения преступления, если при этом преступление не было доведено до конца по не зависящим от этого лица обстоятельствам');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('30 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Уголовная ответственность наступает за приготовление только к тяжкому и особо тяжкому преступлениям');
        -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('30 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Покушением на преступление признаются умышленные действия (бездействие) лица, непосредственно направленные на совершение преступления, если при этом преступление не было доведено до конца по не зависящим от этого лица обстоятельствам');

    -- статья 102
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 0, modern.pkg_scr.getHash('102'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '102',
      'Продление, изменение и прекращение применения принудительных мер медицинского характера');
    select modern.ukitem_seq.currval into curr_state_val from dual;
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('102 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Продление, изменение и прекращение применения принудительных мер медицинского характера осуществляются судом по представлению администрации учреждения, осуществляющего принудительное лечение, на основании заключения комиссии врачей-психиатров');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('102 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Лицо, которому назначена принудительная мера медицинского характера, подлежит освидетельствованию комиссией врачей-психиатров не реже одного раза в шесть месяцев для решения вопроса о наличии оснований для внесения представления в суд о прекращении применения или об изменении такой меры. Освидетельствование такого лица проводится по инициативе лечащего врача, если в процессе лечения он пришел к выводу о необходимости изменения принудительной меры медицинского характера либо прекращения ее применения, а также по ходатайству самого лица, его законного представителя и (или) близкого родственника. Ходатайство подается через администрацию учреждения, осуществляющего принудительное лечение, вне зависимости от времени последнего освидетельствования. При отсутствии оснований для прекращения применения или изменения принудительной меры медицинского характера администрация учреждения, осуществляющего принудительное лечение, представляет в суд заключение для продления принудительного лечения.');
        -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('102 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Изменение или прекращение применения принудительной меры медицинского характера осуществляется судом в случае такого изменения психического состояния лица, при котором отпадает необходимость в применении ранее назначенной меры либо возникает необходимость в назначении иной принудительной меры медицинского характера');
        -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('102 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Изменение или прекращение применения принудительной меры медицинского характера осуществляется судом в случае такого изменения психического состояния лица, при котором отпадает необходимость в применении ранее назначенной меры либо возникает необходимость в назначении иной принудительной меры медицинского характера');

    -- статья 105
      -- часть 1
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 1, modern.pkg_scr.getHash('105 1'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
      'Убийство, то есть умышленное причинение смерти другому человеку');
      -- часть 2
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 1, modern.pkg_scr.getHash('105 2'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
      'Убийство');
      -- пункты части 2
    select modern.ukitem_seq.currval into curr_past_val from dual;
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 а'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
      'двух или более лиц');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 б'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
      'лица или его близких в связи с осуществлением данным лицом служебной деятельности или выполнением общественного долга');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 в'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
      'лица, заведомо для виновного находящегося в беспомощном состоянии, а равно сопряженное с похищением человека');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 г'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
      'женщины, заведомо для виновного находящейся в состоянии беременности');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 д'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
      'совершенное с особой жестокостью');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 е'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'е',
      'совершенное общеопасным способом');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 е.1'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'е.1',
      'по мотиву кровной мести');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 ж'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'ж',
      'совершенное группой лиц, группой лиц по предварительному сговору или организованной группой');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 з'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'з',
      'из корыстных побуждений или по найму, а равно сопряженное с разбоем, вымогательством или бандитизмом');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 и'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'и',
      'из хулиганских побуждений');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 к'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'к',
      'с целью скрыть другое преступление или облегчить его совершение, а равно сопряженное с изнасилованием или насильственными действиями сексуального характера');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 л'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'л',
      'по мотивам политической, идеологической, расовой, национальной или религиозной ненависти или вражды либо по мотивам ненависти или вражды в отношении какой-либо социальной группы');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 м'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'м',
      'в целях использования органов или тканей потерпевшего');
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('105 2 н'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'н',
      'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ наказывается лишением свободы на срок от восьми до двадцати лет, либо пожизненным лишением свободы, либо смертной казнью');

   -- статья 107
      -- часть 1
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 3, modern.pkg_scr.getHash('107 1'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
      'Убийство, совершенное в состоянии внезапно возникшего сильного душевного волнения (аффекта), вызванного насилием, издевательством или тяжким оскорблением со стороны потерпевшего либо иными противоправными или аморальными действиями (бездействием) потерпевшего, а равно длительной психотравмирующей ситуацией, возникшей в связи с систематическим противоправным или аморальным поведением потерпевшего');
      -- часть 2
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 3, modern.pkg_scr.getHash('107 2'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
      'Убийство двух или более лиц, совершенное в состоянии аффекта');

    -- статья 109
      -- часть 1
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 5, modern.pkg_scr.getHash('109 1'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
      'Причинение смерти по неосторожности');
      -- часть 2
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 5, modern.pkg_scr.getHash('109 2'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
      'Причинение смерти по неосторожности вследствие ненадлежащего исполнения лицом своих профессиональных обязанностей');
      -- часть 3
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 5, modern.pkg_scr.getHash('109 3'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
      'Причинение смерти по неосторожности двум или более лицам');

    -- статья 108
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 0, modern.pkg_scr.getHash('108'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '108',
      'Убийство, совершенное при превышении пределов необходимой обороны либо при превышении мер, необходимых для задержания лица, совершившего преступление');
    select modern.ukitem_seq.currval into curr_state_val from dual;
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('108 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Убийство, совершенное при превышении пределов необходимой обороны');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('108 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Убийство, совершенное при превышении мер, необходимых для задержания лица, совершившего преступление');

    -- статья 111
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 7, modern.pkg_scr.getHash('111 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Умышленное причинение тяжкого вреда здоровью, опасного для жизни человека, или повлекшего за собой потерю зрения, речи, слуха либо какого-либо органа или утрату органом его функций, прерывание беременности, психическое расстройство, заболевание наркоманией либо токсикоманией, или выразившегося в неизгладимом обезображивании лица, или вызвавшего значительную стойкую утрату общей трудоспособности не менее чем на одну треть или заведомо для виновного полную утрату профессиональной трудоспособности');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 7, modern.pkg_scr.getHash('111 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Те же деяния, совершенные');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'в отношении лица или его близких в связи с осуществлением данным лицом служебной деятельности или выполнением общественного долга');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'с особой жестокостью, издевательством или мучениями для потерпевшего, а равно в отношении лица, заведомо для виновного находящегося в беспомощном состоянии');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с особой жестокостью, издевательством или мучениями для потерпевшего, а равно в отношении лица, заведомо для виновного находящегося в беспомощном состоянии');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'по найму');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 д'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
          'из хулиганских побуждений');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 е'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'е',
          'по мотивам политической, идеологической, расовой, национальной или религиозной ненависти или вражды либо по мотивам ненависти или вражды в отношении какой-либо социальной группы');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 2 ж'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'ж',
          'в целях использования органов или тканей потерпевшего');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 7, modern.pkg_scr.getHash('111 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        ' Деяния, предусмотренные частями первой или второй настоящей статьи, если они совершены');
      -- пункты части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'группой лиц, группой лиц по предварительному сговору или организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'в отношении двух или более лиц');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('111 3 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
      -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 7, modern.pkg_scr.getHash('111 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Деяния, предусмотренные частями первой, второй или третьей настоящей статьи, повлекшие по неосторожности смерть потерпевшего');

    -- статья 112
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 8, modern.pkg_scr.getHash('112 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Умышленное причинение средней тяжести вреда здоровью, не опасного для жизни человека и не повлекшего последствий, указанных в статье 111 настоящего Кодекса, но вызвавшего длительное расстройство здоровья или значительную стойкую утрату общей трудоспособности менее чем на одну треть');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 8, modern.pkg_scr.getHash('112 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'То же деяние, совершенное');
      -- пункт части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'в отношении двух или более лиц');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'в отношении лица или его близких в связи с осуществлением данным лицом служебной деятельности или выполнением общественного долга');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с особой жестокостью, издевательством или мучениями для потерпевшего, а равно в отношении лица, заведомо для виновного находящегося в беспомощном состоянии');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'группой лиц, группой лиц по предварительному сговору или организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 д'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
          'из хулиганских побуждений');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 е'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'е',
          'по мотивам политической, идеологической, расовой, национальной или религиозной ненависти или вражды либо по мотивам ненависти или вражды в отношении какой-либо социальной группы');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('112 2 ж'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'ж',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');

    -- статья 126
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 24, modern.pkg_scr.getHash('126 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Похищение человека');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 24, modern.pkg_scr.getHash('126 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'То же деяние, совершенное');
      -- пункт части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'группой лиц по предварительному сговору');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с особой жестокостью, издевательством или мучениями для потерпевшего, а равно в отношении лица, заведомо для виновного находящегося в беспомощном состоянии');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'с применением оружия или предметов, используемых в качестве оружия');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 д'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
          'в отношении заведомо несовершеннолетнего');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 е'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'е',
          'в отношении женщины, заведомо для виновного находящейся в состоянии беременности');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 ж'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'ж',
          'в отношении двух или более лиц');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 2 з'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'з',
          'из корыстных побуждений');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 24, modern.pkg_scr.getHash('126 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Деяния, предусмотренные частями первой или второй настоящей статьи, если они');
      -- пункт части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'совершены организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('126 3 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'повлекли по неосторожности смерть потерпевшего или иные тяжкие последствия');

    -- статья 131
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 32, modern.pkg_scr.getHash('131 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Изнасилование, то есть половое сношение с применением насилия или с угрозой его применения к потерпевшей или к другим лицам либо с использованием беспомощного состояния потерпевшей');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 32, modern.pkg_scr.getHash('131 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Изнасилование');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'совершенное группой лиц, группой лиц по предварительному сговору или организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'соединенное с угрозой убийством или причинением тяжкого вреда здоровью, а также совершенное с особой жестокостью по отношению к потерпевшей или к другим лицам');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'повлекшее заражение потерпевшей венерическим заболеванием');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 2 д'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
          'заведомо несовершеннолетней');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 32, modern.pkg_scr.getHash('131 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Изнасилование');
      -- пункт части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'повлекшее по неосторожности смерть потерпевшей');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'повлекшее по неосторожности причинение тяжкого вреда здоровью потерпевшей, заражение ее ВИЧ-инфекцией или иные тяжкие последствия');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('131 3 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'повлекли по неосторожности смерть потерпевшего или иные тяжкие последствия');

    -- статья 132
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 33, modern.pkg_scr.getHash('132 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Мужеложство, лесбиянство или иные действия сексуального характера с применением насилия или с угрозой его применения к потерпевшему (потерпевшей) или к другим лицам либо с использованием беспомощного состояния потерпевшего (потерпевшей)');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 33, modern.pkg_scr.getHash('132 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Те же деяния');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'совершенные группой лиц, группой лиц по предварительному сговору или организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'соединенные с угрозой убийством или причинением тяжкого вреда здоровью, а также совершенные с особой жестокостью по отношению к потерпевшему (потерпевшей) или к другим лицам');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'повлекшие заражение потерпевшего (потерпевшей) венерическим заболеванием');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 2 д'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
          'совершенные в отношении заведомо несовершеннолетнего (несовершеннолетней), - наказываются лишением свободы на срок от четырех до десяти лет');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 33, modern.pkg_scr.getHash('132 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Изнасилование');
      -- пункт части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'повлекли по неосторожности смерть потерпевшего (потерпевшей)');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'повлекшее по неосторожности причинение тяжкого вреда здоровью потерпевшей, заражение ее ВИЧ-инфекцией или иные тяжкие последствия');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('132 3 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          ' совершены в отношении лица, заведомо не достигшего четырнадцатилетнего возраста');

    -- статья 139
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 40, modern.pkg_scr.getHash('139 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Незаконное проникновение в жилище, совершенное против воли проживающего в нем лица');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 40, modern.pkg_scr.getHash('139 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'То же деяние, совершенное с применением насилия или с угрозой его применения');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 40, modern.pkg_scr.getHash('139 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Деяния, предусмотренные частями первой или второй настоящей статьи, совершенные лицом с использованием своего служебного положения');

    -- статья 150
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 54, modern.pkg_scr.getHash('150 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Вовлечение несовершеннолетнего в совершение преступления путем обещаний, обмана, угроз или иным способом, совершенное лицом, достигшим восемнадцатилетнего возраста');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 54, modern.pkg_scr.getHash('150 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'То же деяние, совершенное родителем, педагогом либо иным лицом, на которое законом возложены обязанности по воспитанию несовершеннолетнего');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 54, modern.pkg_scr.getHash('150 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Деяния, предусмотренные частями первой или второй настоящей статьи, совершенные с применением насилия или с угрозой его применения');
      -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 54, modern.pkg_scr.getHash('150 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Деяния, предусмотренные частями первой, второй или третьей настоящей статьи, связанные с вовлечением несовершеннолетнего в преступную группу либо в совершение тяжкого или особо тяжкого преступления, а также в совершение преступления по мотивам политической, идеологической, расовой, национальной или религиозной ненависти или вражды либо по мотивам ненависти или вражды в отношении какой-либо социальной группы');

    -- статья 158
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 62, modern.pkg_scr.getHash('158 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Кража, то есть тайное хищение чужого имущества');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 62, modern.pkg_scr.getHash('158 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Кража, совершенная');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'группой лиц по предварительному сговору');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'с незаконным проникновением в помещение либо иное хранилище');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с причинением значительного ущерба гражданину');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'из одежды, сумки или другой ручной клади, находившихся при потерпевшем');

      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 62, modern.pkg_scr.getHash('158 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Кража, совершенная');
      -- пункты части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'а',
          'с незаконным проникновением в жилище');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'из нефтепровода, нефтепродуктопровода, газопровода');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 3 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'в крупном размере');
      -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 62, modern.pkg_scr.getHash('158 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Кража, совершенная');
      -- пункты части 4
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 4 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('158 4 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'в особо крупном размере');

    -- статья 161
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 65, modern.pkg_scr.getHash('161 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Грабеж, то есть открытое хищение чужого имущества');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 65, modern.pkg_scr.getHash('161 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Грабеж, совершенный');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'группой лиц по предварительному сговору');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'утратил силу');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с незаконным проникновением в жилище, помещение либо иное хранилище');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'с применением насилия, не опасного для жизни или здоровья, либо с угрозой применения такого насилия');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 2 д'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'д',
          'в крупном размере');

      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 65, modern.pkg_scr.getHash('161 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Грабеж, совершенный');
      -- пункты части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('161 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'в особо крупном размере');

    -- статья 162
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 66, modern.pkg_scr.getHash('162 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Разбой, то есть нападение в целях хищения чужого имущества, совершенное с применением насилия, опасного для жизни или здоровья, либо с угрозой применения такого насилия');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 66, modern.pkg_scr.getHash('162 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Разбой, совершенный группой лиц по предварительному сговору, а равно с применением оружия или предметов, используемых в качестве оружия');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 66, modern.pkg_scr.getHash('162 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Разбой, совершенный с незаконным проникновением в жилище, помещение либо иное хранилище или в крупном размере');
      -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 66, modern.pkg_scr.getHash('162 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Разбой, совершенный');
      -- пункты части 4
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('162 4 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('162 4 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'в целях завладения имуществом в особо крупном размере');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('162 4 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'причинением тяжкого вреда здоровью потерпевшего');

    -- статья 166
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 0, modern.pkg_scr.getHash('166'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '166',
      'Неправомерное завладение автомобилем или иным транспортным средством без цели хищения');
    select modern.ukitem_seq.currval into curr_state_val from dual;
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('166 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Неправомерное завладение автомобилем или иным транспортным средством без цели хищения (угон)');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('166 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'То же деяние, совершенное');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('166 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'группой лиц по предварительному сговору');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('166 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('166 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с применением насилия, не опасного для жизни или здоровья, либо с угрозой применения такого насилия');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('166 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Деяния, предусмотренные частями первой или второй настоящей статьи, совершенные организованной группой либо причинившие особо крупный ущерб');
      -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('166 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Деяния, предусмотренные частями первой, второй или третьей настоящей статьи, совершенные с применением насилия, опасного для жизни или здоровья, либо с угрозой применения такого насилия');

    -- статья 167
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 71, modern.pkg_scr.getHash('167 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Умышленные уничтожение или повреждение чужого имущества, если эти деяния повлекли причинение значительного ущерба');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 71, modern.pkg_scr.getHash('167 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Те же деяния, совершенные из хулиганских побуждений, путем поджога, взрыва или иным общеопасным способом либо повлекшие по неосторожности смерть человека или иные тяжкие последствия');

    -- статья 228
    insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 0, modern.pkg_scr.getHash('228'));
    insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '228',
      'Незаконные приобретение, хранение, перевозка, изготовление, переработка наркотических средств, психотропных веществ или их аналогов');
    select modern.ukitem_seq.currval into curr_state_val from dual;
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('228 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Незаконные приобретение, хранение, перевозка, изготовление, переработка без цели сбыта наркотических средств, психотропных веществ или их аналогов в крупном размере');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_state_val, modern.pkg_scr.getHash('228 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Те же деяния, совершенные в особо крупном размере');

    -- статья 229
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 143, modern.pkg_scr.getHash('229 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Хищение либо вымогательство наркотических средств или психотропных веществ - наказываются лишением свободы на срок от трех до семи лет');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 143, modern.pkg_scr.getHash('229 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Те же деяния, совершенные');
      -- пункты части 2
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 2 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'группой лиц по предварительному сговору');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 2 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 2 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'лицом с использованием своего служебного положения');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 2 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'с применением насилия, не опасного для жизни или здоровья, либо с угрозой применения такого насилия');

      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 143, modern.pkg_scr.getHash('229 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Деяния, предусмотренные частями первой или второй настоящей статьи, если они совершены');
      -- пукнты части 3
      select modern.ukitem_seq.currval into curr_past_val from dual;
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 3 а'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'a',
          'организованной группой');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 3 б'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'б',
          ' в отношении наркотических средств или психотропных веществ в крупном размере');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 3 в'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'в',
          'с применением насилия, опасного для жизни или здоровья, либо с угрозой применения такого насилия');
        insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, curr_past_val, modern.pkg_scr.getHash('229 3 г'));
        insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, 'г',
          'утратил силу. - Федеральный закон от 08.12.2003 N 162-ФЗ');

    -- статья 264
      -- часть 1
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 181, modern.pkg_scr.getHash('264 1'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '1',
        'Нарушение лицом, управляющим автомобилем, трамваем либо другим механическим транспортным средством, правил дорожного движения или эксплуатации транспортных средств, повлекшее по неосторожности причинение тяжкого вреда здоровью человека');
      -- часть 2
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 181, modern.pkg_scr.getHash('264 2'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '2',
        'Деяние, предусмотренное частью первой настоящей статьи, совершенное лицом, находящимся в состоянии опьянения, повлекшее по неосторожности причинение тяжкого вреда здоровью человека');
      -- часть 3
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 181, modern.pkg_scr.getHash('264 3'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '3',
        'Деяние, предусмотренное частью первой настоящей статьи, совершенное лицом, находящимся в состоянии опьянения, повлекшее по неосторожности причинение тяжкого вреда здоровью человека');
      -- часть 4
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 181, modern.pkg_scr.getHash('264 4'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '4',
        'Деяние, предусмотренное частью первой настоящей статьи, совершенное лицом, находящимся в состоянии опьянения, повлекшее по неосторожности смерть человека');
      -- часть 5
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 181, modern.pkg_scr.getHash('264 5'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '5',
        'Деяние, предусмотренное частью первой настоящей статьи, повлекшее по неосторожности смерть двух или более лиц');
      -- часть 6
      insert into modern.ukitem(id, parent_id, hash) values (modern.ukitem_seq.nextval, 181, modern.pkg_scr.getHash('264 6'));
      insert into modern.ukarticl(id, artcl, note) values (modern.ukitem_seq.currval, '6',
        'Деяние, предусмотренное частью первой настоящей статьи, совершенное лицом, находящимся в состоянии опьянения, повлекшее по неосторожности смерть двух или более лиц');
  end;
end;
/

prompt
prompt Creating package body PKG_SCR
prompt =============================
prompt
create or replace package body modern.PKG_SCR is
  function Encrypt(a_str in nvarchar2, a_key in nvarchar2) return raw is
  begin
    return dbms_crypto.Encrypt(utl_raw.cast_to_raw(a_str), dbms_crypto.DES_CBC_PKCS5,
      utl_raw.cast_to_raw(a_key));
  end;
  function Decrypt(a_raw in raw, a_key in nvarchar2) return varchar2 is
  begin
    return utl_raw.cast_to_varchar2(
      dbms_crypto.Decrypt(a_raw, dbms_crypto.DES_CBC_PKCS5, utl_raw.cast_to_raw(a_key)));
  end;
  function getHash(a_str in nvarchar2) return raw is
  begin
    return dbms_crypto.Hash(utl_raw.cast_to_raw(a_str), dbms_crypto.HASH_SH1);
  end;
  function user_right(a_user_id in number) return tbl_list_number pipelined is
  begin
    for c in (
      select func_id
        from (select ar.func_id, 1 perm
                from modern.roles_expert rle, modern.action_roles ar
               where ar.role_id = rle.role_id
                 and rle.expert_id = a_user_id
              union all
              select re.func_id, re.permission perm
                from modern.action_expet re
               where expert_id = a_user_id)
       group by func_id
      having sum(perm) > 0
      )
     loop
        pipe row(t_list_number(c.func_id, ''));
     end loop;
  end;
end;
/

prompt
prompt Creating package body PRK_TAB
prompt =============================
prompt
create or replace package body modern.prk_tab is

  -- Работа со справочниками
    -- Справочник "Коды МВД"
    function mvd_ins(a_name in nvarchar2, a_short_name in nvarchar2, a_code in nvarchar2) return number is
      cnt number default 0;
    begin
      select modern.code_mvd_seq.nextval into cnt from dual;
      insert into modern.code_mvd(id, name, short_name, code) values (cnt, a_name, a_short_name, a_code);
      return cnt;
    end;
    procedure mvd_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2, a_code in nvarchar2) is
    begin
      update modern.code_mvd cm set
        cm.name = a_name,
        cm.short_name = a_short_name,
        cm.code = a_code
      where
        cm.id = a_id;
    end;
    procedure mvd_del(a_id number) is
    begin
      delete from modern.code_mvd where id = a_id;
    end;

    -- Справочник "Коды райлиноргана"
    function lin_ins(a_code in nvarchar2, a_organ in nvarchar2) return number is
      cnt number default 0;
    begin
      select modern.code_lin_seq.nextval into cnt from dual;
      insert into modern.code_lin(id, code, organ) values (cnt, a_code, a_organ);
      return cnt;
    end;
    procedure lin_upd(a_id in number, a_code in nvarchar2, a_organ in nvarchar2) is
    begin
      update modern.code_lin cl set
        cl.code = a_code,
        cl.organ = a_organ
       where
        cl.id = a_id;
    end;
    procedure lin_del(a_id number) is
    begin
      delete from modern.code_lin where id = a_id;
    end;

    -- Справочник "Список экспертных подразделений"
    function division_ins(a_name in nvarchar2, a_address in nvarchar2, a_phone in nvarchar2) return number is
      cnt number default 0;
    begin
      select modern.division_seq.nextval into cnt from dual;
      insert into modern.division(id, name, address, phone) values (cnt, a_name, a_address, a_phone);
      return cnt;
    end;
    procedure division_upd(a_id in number, a_name in nvarchar2, a_address in nvarchar2, a_phone in nvarchar2) is
    begin
      update modern.division d set
        d.name = a_name,
        d.address = a_address,
        d.phone = a_phone
      where
        d.id = a_id;
    end;
    procedure division_del(a_id in number) is
    begin
      delete from modern.division where id = a_id;
    end;

      -- Список должностей
    function post_ins(a_name in nvarchar2) return number is
      cnt number default 0;
    begin
      select modern.post_seq.nextval into cnt from dual;
      insert into modern.post(id, name) values (cnt, a_name);
      return cnt;
    end;
    procedure post_upd(a_id in number, a_name in nvarchar2) is
    begin
      update modern.post p set
        p.name = a_name
      where
        id = a_id;
    end;
    procedure post_del(a_id in number) is
    begin
      delete from modern.post where id = a_id;
    end;

    -- Список экспертов
    function exp_ins(a_div_id in number, a_post_id in number, a_surname in nvarchar2,
      a_name in nvarchar2, a_patronic in nvarchar2, a_login in nvarchar2, a_sign in nvarchar2) return number is
      cnt number default 0;
      salt nvarchar2(4);
    begin
      select modern.expert_seq.nextval into cnt from dual;
      salt := dbms_random.string('a', 4);
      insert into modern.expert(id, division_id, post_id, surname, name, patronic, login, sign, salt)
        values (cnt, a_div_id, a_post_id, a_surname, a_name, a_patronic, a_login, a_sign, salt);
      return cnt;
    end;
    procedure exp_upd(a_id in number, a_div_id in number, a_post_id in number, a_surname in nvarchar2,
      a_name in nvarchar2, a_patronic in nvarchar2, a_login in nvarchar2, a_sign in nvarchar2) is
    begin
      update modern.expert e set
        e.division_id = a_div_id,
        e.post_id = a_post_id,
        e.surname = a_surname,
        e.name = a_name,
        e.patronic = a_patronic,
        e.login = a_login,
        e.sign = a_sign
      where
        e.id = a_id;
    end;
    procedure exp_del(a_id in number, a_curr_user in number) is
      p_expert nvarchar2(256);
      hist_id number;
    begin
      select 'id='||a_id||';name='||surname||' '||name||' '||patronic||';division='||division||';post='||post||';login='||login||';sign='||sign
        into p_expert from modern.v_expert ve
        where ve.id = a_id;
      hist_id := pkg_card.set_card_history(null, 8, a_curr_user, p_expert);

      delete from modern.action_expet ae where ae.expert_id = a_id;
      delete from modern.roles_expert re where re.expert_id = a_id;
      delete from modern.expert where id = a_id;
    end;

    procedure exp_change_password(a_id in number, a_password in nvarchar2) is
    begin
      update modern.expert e
      set e.password = modern.PKG_SCR.getHash(a_password||salt)
      where e.id = a_id;
    end;

    function exp_check_password(a_id in number, a_password in nvarchar2) return number is
      p_password nvarchar2(512);
      p_salt nvarchar2(4);
    begin
      select password, salt into p_password, p_salt from modern.expert where id = a_id;
      if (modern.PKG_SCR.getHash(a_password||p_salt) = p_password) then
        return 1;
      else
        return 0;
      end if;
    end;

    -- Справочник "Список методов расчета"
    function method_ins(a_name in nvarchar2, a_def_freq in number, a_desc in nvarchar2) return number is
      cnt number default 0;
    begin
      select modern.method_seq.nextval into cnt from dual;
      insert into modern.method(id, name, description, def_freq) values (cnt, a_name, a_desc, a_def_freq);
      return cnt;
    end;

    procedure freq_ins(a_allele_id in number, a_method_id in number, a_freq in number) is
    begin
      insert into modern.allele_freq(allele_id, method_id, freq) values (a_allele_id, a_method_id, a_freq);
    end;

    procedure method_upd(a_id in number, a_name in nvarchar2, a_def_freq in number, a_desc in nvarchar2) is
    begin
      update modern.method m set
        m.name = a_name,
        m.description = a_desc,
        m.def_freq = a_def_freq
      where
        m.id = a_id;
    end;

    procedure freq_upd(a_method_id in number, a_allele_id in number, a_freq in number) is
    begin
      update modern.allele_freq af set
        af.freq = a_freq
      where
        af.allele_id = a_allele_id and af.method_id = a_method_id;
    end;

    procedure method_del(a_id in number) is
    begin
      methods_freq_del(a_id);
      delete from modern.method where id = a_id;
    end;

    procedure methods_freq_del(a_id in number) is
    begin
      for rec in (
        select af.allele_id, af.method_id from modern.allele_freq af
          where af.method_id = a_id
      ) loop
        freq_del(rec.method_id, rec.allele_id);
      end loop;
    end;

    procedure freq_del(a_method_id in number, a_allele_id in number) is
    begin
      delete from modern.allele_freq af
        where
          af.allele_id = a_allele_id and af.method_id = a_method_id;
    end;

    -- Справочник "Статьи УК"
    function uk_artcl_ins(a_artcl in nvarchar2, a_note in nvarchar2, a_parent_id in number) return number is
      cnt number;
    begin
      select  modern.ukitem_seq.nextval into cnt from dual;
      insert into modern.ukitem(id, parent_id) values (cnt, a_parent_id);
      insert into modern.ukarticl(id, artcl, note) values (cnt, a_artcl, a_note);
      return cnt;
    end;

    function uk_text_ins(a_note in nvarchar2, a_parent_id in number) return number is
      cnt number;
    begin
      select  modern.ukitem_seq.nextval into cnt from dual;
      insert into modern.ukitem(id, parent_id) values (cnt, a_parent_id);
      insert into modern.uktext(id, note) values (cnt, a_note);
      return cnt;
    end;

    procedure uk_artcl_upd(a_id in number, a_artcl in nvarchar2, a_note in nvarchar2, a_parent_id in number) is
    begin
      update modern.ukarticl u set
        u.artcl = a_artcl,
        u.note = a_note
      where
        u.id = a_id;

      update modern.ukitem i set
        i.parent_id = a_parent_id
      where
        i.id = a_id;
    end;

    procedure uk_text_upd(a_id in number, a_note in nvarchar2, a_parent_id in number) is
    begin
      update modern.uktext t set
        t.note = a_note
      where
        t.id = a_id;

      update modern.ukitem i set
        i.parent_id = a_parent_id
      where
        i.id = a_id;
    end;

    procedure uk_item_del(a_id in number) is
    begin
      -- определено каскадное удаление
      delete from modern.ukitem where id = a_id;
    end;

    -- Справочник "Организации"
    function org_ins(a_note in nvarchar2) return number is
      cnt number;
    begin
      select modern.organization_seq.nextval into cnt from dual;
      insert into modern.organization(id, note) values (cnt, a_note);
      return cnt;
    end;
    procedure org_upd(a_id in number, a_note in nvarchar2) is
    begin
      update modern.organization
      set note = a_note
      where id = a_id;
    end;
    procedure org_del(a_id in number) is
    begin
      delete from modern.organization where id = a_id;
    end;

    -- Справочник "Вид объекта"
    function sort_ins(a_name in nvarchar2, a_short_name in nvarchar2) return number is
      cnt number;
    begin
      select modern.sort_object_seq.nextval into cnt from dual;
      insert into modern.sort_object(id, name, short_name) values
        (cnt, a_name, a_short_name);
      return cnt;
    end;
    procedure sort_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2) is
    begin
      update modern.sort_object so
      set so.name = a_name,
        so.short_name = a_short_name
      where so.id = a_id;
    end;
    procedure sort_del(a_id in number) is
    begin
      delete from modern.sort_object where id = a_id;
    end;

    -- Справочник "Категории объекта"
    function class_obj_ins(a_name in nvarchar2, a_short_name in nvarchar2) return number is
      cnt number;
    begin
      select modern.class_object_seq.nextval into cnt from dual;
      insert into modern.class_object(id, name, short_name) values (cnt, a_name, a_short_name);
      return cnt;
    end;
    procedure class_obj_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2) is
    begin
      update modern.class_object co
      set co.name = a_name, co.short_name = a_short_name
      where co.id = a_id;
    end;
    procedure class_obj_del(a_id in number) is
    begin
      delete from modern.class_object where id = a_id;
    end;

    -- Справочник "Категории карточки ИКЛ"
    function class_ikl_ins(a_name in nvarchar2, a_short_name in nvarchar2) return number is
      cnt number;
    begin
      select modern.class_object_seq.nextval into cnt from dual;
      insert into modern.class_ikl(id, name, short_name) values (cnt, a_name, a_short_name);
      return cnt;
    end;
    procedure class_ikl_upd(a_id in number, a_name in nvarchar2, a_short_name in nvarchar2) is
    begin
      update modern.class_ikl cl
      set cl.name = a_name, cl.short_name = a_short_name
      where cl.id = a_id;
    end;
    procedure class_ikl_del(a_id in number) is
    begin
      delete from modern.class_ikl where id = a_id;
    end;

    -- Список ролей
    function role_ins(a_name in nvarchar2) return number is
      cnt number default 0;
    begin
      select modern.roles_seq.nextval into cnt from dual;
      insert into modern.roles(id, name) values (cnt, a_name);
      return cnt;
    end;
    procedure role_upd(a_id in number, a_name in nvarchar2) is
    begin
      update modern.roles p set
        p.name = a_name
      where
        id = a_id;
    end;
    procedure role_del(a_id in number) is
    begin
      delete from modern.roles_expert re where re.role_id = a_id;
      delete from modern.action_roles ar where ar.role_id = a_id;
      delete from modern.roles where id = a_id;
    end;

    -- Назначение прав ролям
    procedure role_right_del(a_role_id in number) is
    begin
      delete from modern.action_roles where role_id = a_role_id;
      null;
    end;

    procedure role_right_ins(a_role_id in number, a_func_id in number) is
    begin
      insert into modern.action_roles(role_id, func_id) values (a_role_id, a_func_id);
    end;

    -- Назначение прав пользователям
    procedure right_user_del(a_user_id in number) is
    begin
      delete from modern.roles_expert re where re.expert_id = a_user_id;
      delete from modern.action_expet ae where ae.expert_id = a_user_id;
    end;
    procedure right_user_ins(a_user_id in number, a_func_id in number, a_permission in number) is
    begin
      insert into modern.action_expet(func_id, expert_id, permission) values (a_func_id, a_user_id, a_permission);
    end;
    procedure role_user_ins(a_user_id in number, a_role_id in number) is
    begin
      insert into modern.roles_expert (role_id, expert_id) values (a_role_id, a_user_id);
    end;

    -- Работа с фильтрами
    function filter_ins(a_name in nvarchar2, a_text in nclob) return number is
      cnt number;
    begin
      select modern.filters_seq.nextval into cnt from dual;
      insert into modern.filters(id, name, text) values (cnt, a_name, a_text);
      return cnt;
    end;
    procedure filter_upd(a_id in number, a_name in nvarchar2, a_text in nvarchar2) is
    begin
      update modern.filters f
      set f.name = a_name, f.text = a_text
      where f.id = a_id;
    end;
    procedure filter_del(a_id in number) is
    begin
      delete from modern.filters where id = a_id;
    end;

end;
/

prompt
prompt Creating type body T_STRING_AGG
prompt ===============================
prompt
CREATE OR REPLACE TYPE BODY MODERN.t_string_agg IS
  STATIC FUNCTION ODCIAggregateInitialize(sctx IN OUT t_string_agg) RETURN NUMBER IS
  BEGIN
    sctx := t_string_agg(NULL);
    RETURN ODCIConst.Success;
  END;

  MEMBER FUNCTION ODCIAggregateIterate(self IN OUT t_string_agg, value IN VARCHAR2) RETURN NUMBER IS
  BEGIN
    SELF.g_string := self.g_string || ',' || value;
    RETURN ODCIConst.Success;
  END;

  MEMBER FUNCTION ODCIAggregateTerminate(self IN   t_string_agg,returnValue OUT VARCHAR2, flags IN NUMBER) RETURN NUMBER IS
  BEGIN
    returnValue := RTRIM(LTRIM(SELF.g_string, ','), ',');
    RETURN ODCIConst.Success;
  END;

  MEMBER FUNCTION ODCIAggregateMerge(self IN OUT t_string_agg, ctx2 IN t_string_agg) RETURN NUMBER IS
  BEGIN
    SELF.g_string := SELF.g_string || ',' || ctx2.g_string;
    RETURN ODCIConst.Success;
  END;
END;
/

prompt
prompt Creating type body T_STRING_AGG_SPACE
prompt =====================================
prompt
CREATE OR REPLACE TYPE BODY MODERN.t_string_agg_space IS
  STATIC FUNCTION ODCIAggregateInitialize(sctx IN OUT t_string_agg_space) RETURN NUMBER IS
  BEGIN
    sctx := t_string_agg_space(NULL);
    RETURN ODCIConst.Success;
  END;

  MEMBER FUNCTION ODCIAggregateIterate(self IN OUT t_string_agg_space, value IN VARCHAR2) RETURN NUMBER IS
  BEGIN
    SELF.g_string := self.g_string || ' ' || value;
    RETURN ODCIConst.Success;
  END;

  MEMBER FUNCTION ODCIAggregateTerminate(self IN t_string_agg_space, returnValue OUT VARCHAR2, flags IN NUMBER) RETURN NUMBER IS
  BEGIN
    returnValue := RTRIM(LTRIM(SELF.g_string, ','), ',');
    RETURN ODCIConst.Success;
  END;

  MEMBER FUNCTION ODCIAggregateMerge(self IN OUT t_string_agg_space, ctx2 IN t_string_agg_space) RETURN NUMBER IS
  BEGIN
    SELF.g_string := SELF.g_string || ' ' || ctx2.g_string;
    RETURN ODCIConst.Success;
  END;
END;
/


spool off
