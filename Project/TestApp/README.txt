Instructions:
1) Create a database called 'test' in PostgreSQL and create a user with name 'postgres' and password 'admin'. (or change the connection string in the App.config)
3) Run the following queries to create the required database tables

CREATE TABLE account
(
  id bigserial NOT NULL,
  bk text,
  data jsonb,
  version integer,
  CONSTRAINT account_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE account
  OWNER TO postgres;


CREATE TABLE domaineventspublishedtracker
(
  id bigserial NOT NULL,
  bk text,
  data jsonb,
  version integer,
  CONSTRAINT domaineventspublishedtracker_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE domaineventspublishedtracker
  OWNER TO postgres;


CREATE TABLE storeddomainevent
(
  id bigserial NOT NULL,
  bk text,
  data jsonb,
  version integer,
  CONSTRAINT storeddomainevent_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE storeddomainevent
  OWNER TO postgres;
