USE Prueba;

IF OBJECT_ID('tempdb..#Venta', 'U') IS NOT NULL
drop TABLE #Venta
SELECT *
INTO
	#Venta
FROM
	Venta V
WHERE
	V.Fecha BETWEEN DATEADD(DAY, -30, GETDATE()) AND GETDATE()

--Obtener el total de ventas de los últimos 30 días (monto total y cantidad total de ventas).
SELECT
	SUM(VD.Precio_Unitario * VD.Cantidad) AS Monto_Total
	, SUM(VD.Cantidad)                    AS Cantidad_Total_Ventas
FROM
	VentaDetalle VD
	INNER JOIN
		#Venta V
		ON
			V.ID_Venta = VD.ID_Venta

--Obtener el día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).
SELECT
	TOP 1 V.Total                 AS Monto_Max
	, DAY(V.Fecha)                AS Dia
	, FORMAT(V.Fecha, 'HH:mm:ss') AS Hora
FROM
	#Venta V
	INNER JOIN
		VentaDetalle VD
		ON
			V.ID_Venta = VD.ID_Venta
ORDER BY
	V.Total DESC

--Obtener el producto con mayor monto total de ventas.
SELECT
	TOP 1 P.Nombre       as Producto_Nombre
	, SUM(vd.TotalLinea) AS MontoTotalVentas
FROM
	VentaDetalle vd
	INNER JOIN
		#Venta v
		ON
			vd.ID_Venta = V.ID_Venta
	INNER JOIN
		Producto p
		ON
			vd.ID_Producto = p.ID_Producto
GROUP BY
	p.Nombre
ORDER BY
	SUM(vd.TotalLinea) DESC
;

--Obtener el local con mayor monto de ventas.
SELECT
	TOP 1 L.Nombre AS Local_Nombre
	, SUM(V.Total) AS Monto_Total_Ventas
FROM
	#Venta V
	JOIN
		Local L
		ON
			V.ID_Local = L.ID_Local
WHERE
	V.Fecha BETWEEN DATEADD(DAY, -30, GETDATE()) AND GETDATE()
GROUP BY
	L.Nombre
ORDER BY
	SUM(V.Total) DESC
;

--Obtener la marca con mayor margen de ganancias
SELECT
	TOP 1 M.Nombre                                              AS Marca
	, SUM((VD.Precio_Unitario - P.Costo_Unitario) * VD.Cantidad)AS Margen_Ganancias
FROM
	VentaDetalle VD
	INNER JOIN
		Producto P
		ON
			VD.ID_Producto = P.ID_Producto
	INNER JOIN
		Marca M
		ON
			P.ID_Marca = M.ID_Marca
	INNER JOIN
		#Venta V
		ON
			VD.ID_Venta = V.ID_Venta
GROUP BY
	M.Nombre
ORDER BY
	SUM(VD.Precio_Unitario - P.Costo_Unitario) DESC
;

--Obtener cuál es el producto que más se vende en cada local
WITH VentasPorLocalYProducto AS
(
	SELECT
		l.ID_Local
		, l.Nombre         AS Local
		, p.Nombre         AS Producto
		, SUM(vd.Cantidad) AS CantidadTotalVendida
		, DENSE_RANK() OVER (PARTITION BY l.ID_Local ORDER BY
								SUM(vd.Cantidad) DESC) AS Rank
	FROM
		VentaDetalle vd
		INNER JOIN
			#Venta v
			ON
				v.ID_Venta = vd.ID_Venta
		INNER JOIN
			Producto p
			ON
				vd.ID_Producto = p.ID_Producto
		INNER JOIN
			Local l
			ON
				v.ID_Local = l.ID_Local
	GROUP BY
		l.ID_Local
		, l.Nombre
		, p.Nombre
)
SELECT
	Local
	, Producto
	, CantidadTotalVendida
FROM
	VentasPorLocalYProducto
WHERE
	Rank = 1
ORDER BY
	ID_Local
	, CantidadTotalVendida DESC
;