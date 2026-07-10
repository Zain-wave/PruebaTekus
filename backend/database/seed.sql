INSERT INTO [Providers] ([Nit], [Name], [Website], [Email], [Country]) VALUES
(N'900111222', N'Importaciones Tekus S.A.', N'https://importacionestekus.com', N'contacto@importacionestekus.com', N'Colombia'),
(N'900222333', N'Servicios Cuanticos del Valle', N'https://cuanticosvalle.co', N'info@cuanticosvalle.co', N'Colombia'),
(N'900333444', N'Bytes y Bandidos Ltda', N'https://bytesybandidos.com', N'ventas@bytesybandidos.com', N'Mexico'),
(N'900444555', N'Nebulosa Consultores S.A.S.', N'https://nebulosaconsultores.com', N'hola@nebulosaconsultores.com', N'Colombia'),
(N'900555666', N'Tornado de Datos S.A.S.', N'https://tornadodedatos.com', N'contacto@tornadodedatos.com', N'Peru'),
(N'900666777', N'Fabrica de Pixeles Ltda', N'https://fabricadepixeles.com', N'soporte@fabricadepixeles.com', N'Chile'),
(N'900777888', N'Circuitos del Caribe S.A.', N'https://circuitoscaribe.com', N'info@circuitoscaribe.com', N'Colombia'),
(N'900888999', N'Andes Digital Corp', N'https://andesdigital.com', N'contacto@andesdigital.com', N'Peru'),
(N'900999000', N'Selva de Servidores S.A.S.', N'https://selvadeservidores.com', N'ventas@selvadeservidores.com', N'Brasil'),
(N'901000111', N'Paramo Tech Ltda', N'https://paramotech.com', N'hola@paramotech.com', N'Colombia'),
(N'901111222', N'Caribe Cloud Solutions', N'https://caribecloud.com', N'contacto@caribecloud.com', N'Mexico'),
(N'901222333', N'Pacifico Bytes S.A.', N'https://pacificobytes.com', N'info@pacificobytes.com', N'Chile');

INSERT INTO [Services] ([Name], [HourlyRate], [ProviderId])
SELECT v.[Name], v.[HourlyRate], p.[Id]
FROM (VALUES
    (N'Descarga espacial de contenidos', 85.50, N'900111222'),
    (N'Desaparicion forzada de bytes', 92.00, N'900111222'),
    (N'Teletransporte de paquetes TCP', 78.25, N'900222333'),
    (N'Encriptacion cuantica express', 110.00, N'900222333'),
    (N'Extraccion de bits perdidos', 65.00, N'900333444'),
    (N'Rescate de datos secuestrados', 99.99, N'900333444'),
    (N'Migracion a la nube nebulosa', 120.00, N'900444555'),
    (N'Backup en tormenta', 55.75, N'900555666'),
    (N'Restauracion post-tornado', 88.00, N'900555666'),
    (N'Render de pixeles artesanales', 70.50, N'900666777'),
    (N'Compresion de imagenes tropicales', 60.00, N'900777888'),
    (N'Soporte tecnico de altura', 95.00, N'900888999'),
    (N'Hosting en la selva', 45.00, N'900999000'),
    (N'Monitoreo de servidores de paramo', 80.00, N'901000111'),
    (N'Streaming caribeno 4K', 130.00, N'901111222'),
    (N'Auditoria de bytes pacificos', 105.00, N'901222333')
) AS v([Name], [HourlyRate], [Nit])
JOIN [Providers] p ON p.[Nit] = v.[Nit];
