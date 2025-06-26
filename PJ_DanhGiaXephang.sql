create table DanhSach
(
    ID            int identity
        constraint PK_DanhSach
            primary key clustered,
    TenVanBan     nvarchar(max),
    SoVanBan      nvarchar(max),
    TenPhongBan   nvarchar(max),
    TenLoaiVanBan nvarchar(max),
    NgayCapNhat   datetime
);

create table Log_Setting
(
    ID        int identity
        constraint PK_Log_Setting
            primary key clustered,
    KindOfLog nvarchar(max),
    NoteLog   nvarchar(max),
    DateLog   datetime,
    UserName  nvarchar(max)
);

create table lp_function
(
    id   int identity
        constraint PK_Function
            primary key clustered,
    name nvarchar(50) not null,
    code int          not null
        constraint UK_UniFuncCode
            unique nonclustered,
    note nvarchar(max)
);

create table lp_group
(
    id   int identity
        constraint PK_Group
            primary key clustered,
    name nvarchar(200) not null,
    note nvarchar(max)
);

create table lp_groupuser
(
    id       int identity
        constraint PK_GroupUser
            primary key clustered,
    group_id int           not null,
    email    nvarchar(200) not null,
    madonvi  nvarchar(200) not null,
    constraint UK_UniMember
        unique nonclustered (group_id asc, email asc)
);

create table lp_page
(
    id         int identity
        constraint PK_Page
            primary key clustered,
    name       nvarchar(200),
    alias      varchar(100) not null
        constraint UK_UniPageAlias
            unique nonclustered,
    permission int,
    note       nvarchar(max)
);

create table lp_permission
(
    id         int identity
        constraint PK_Permission
            primary key clustered,
    group_id   int not null,
    page_id    int not null,
    permission int not null,
    constraint UK_UniPermission
        unique nonclustered (group_id asc, page_id asc)
);

create table std_evaluation
(
    id         int identity
        constraint std_evaluation_pk
            primary key clustered,
    name       nvarchar(255),
    [desc]     nvarchar(500),
    start_date datetime,
    end_date   datetime,
    created_at datetime,
    updated_at datetime,
    created_by int,
    updated_by int
);

create table std_evaluation_session
(
    id                   int identity
        constraint std_evaluation_session_pk
            primary key clustered,
    evaluation_id        int
        constraint std_evaluation_session_std_evaluation_id_fk
            references std_evaluation (id),
    created_at           datetime,
    updated_at           datetime,
    created_by           int,
    updated_by           int,
    [desc]               nvarchar(500),
    status               tinyint,
    assigned_departments varchar(2048)
);

create table std_column_cell_history
(
    id                    int identity
        constraint std_column_cell_history_pk
            primary key clustered,
    column_id             int,
    evaluation_session_id int
        constraint std_column_cell_history_evaluation_session_id_fk
            references std_evaluation_session (id),
    value                 nvarchar(500),
    [order]               int,
    created_at            datetime,
    updated_at            datetime,
    created_by            int,
    updated_by            int
);

create table std_standard
(
    id              int identity
        constraint standard_pk
            primary key clustered,
    name            nvarchar(255) not null,
    normalized_name varchar(255)  not null,
    created_at      datetime,
    updated_at      datetime,
    created_by      int,
    updated_by      int,
    total_table     int,
    status          tinyint,
    [desc]          nvarchar(500)
);

create table std_evaluation_session_standard
(
    id                    int identity
        constraint std_evaluation_session_standard_pk
            primary key clustered,
    evaluation_session_id int
        constraint std_evaluation_session_standard_evaluation_session_id_fk
            references std_evaluation_session (id)
            on delete cascade,
    standard_id           int
        constraint std_evaluation_session_standard_standard_id_fk
            references std_standard (id)
            on delete cascade,
    created_at            datetime,
    updated_at            datetime,
    created_by            int,
    updated_by            int
);

create table std_standard_table
(
    id          int identity
        constraint std_standard_table_pk
            primary key clustered,
    standard_id int
        constraint standard_table_standard_id_fk
            references std_standard (id)
            on delete cascade,
    name        nvarchar(255),
    created_at  datetime,
    updated_at  datetime,
    created_by  int,
    updated_by  int
);

create table std_standard_table_history
(
    id                    int identity
        constraint std_standard_table_history_pk
            primary key clustered,
    table_id              int,
    evaluation_session_id int
        constraint std_standard_table_history_evaluation_session_id_fk
            references std_evaluation_session (id)
            on delete cascade,
    standard_id           int
        constraint std_standard_table_history_standard_id_fk
            references std_standard (id),
    name                  nvarchar(255),
    created_at            datetime,
    updated_at            datetime,
    created_by            int,
    updated_by            int
);

create table std_table_column
(
    id         int identity
        constraint std_table_column_pk
            primary key clustered,
    table_id   int
        constraint table_column_standard_table_id_fk
            references std_standard_table (id)
            on delete cascade,
    name       nvarchar(255),
    [order]    int,
    created_at datetime,
    updated_at datetime,
    created_by int,
    updated_by int,
    type       int
);

create table std_table_column_history
(
    id                    int identity
        constraint std_table_column_history_pk
            primary key clustered,
    column_id             int,
    table_id              int,
    evaluation_session_id int
        constraint std_table_column_history_evaluation_session_id_fk
            references std_evaluation_session (id),
    name                  nvarchar(255),
    [order]               int,
    created_at            datetime,
    updated_at            datetime,
    created_by            int,
    updated_by            int,
    type                  int
);


