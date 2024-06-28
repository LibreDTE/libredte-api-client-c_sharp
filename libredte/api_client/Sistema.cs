/*
 * LibreDTE: Cliente de API en C#.
 * Copyright (C) LibreDTE <https://www.libredte.cl>
 *
 * Este programa es software libre: usted puede redistribuirlo y/o modificarlo
 * bajo los términos de la GNU Lesser General Public License (LGPL) publicada
 * por la Fundación para el Software Libre, ya sea la versión 3 de la Licencia,
 * o (a su elección) cualquier versión posterior de la misma.
 *
 * Este programa se distribuye con la esperanza de que sea útil, pero SIN
 * GARANTÍA ALGUNA; ni siquiera la garantía implícita MERCANTIL o de APTITUD
 * PARA UN PROPÓSITO DETERMINADO. Consulte los detalles de la GNU Lesser General
 * Public License (LGPL) para obtener una información más detallada.
 *
 * Debería haber recibido una copia de la GNU Lesser General Public License
 * (LGPL) junto a este programa. En caso contrario, consulte
 * <http://www.gnu.org/licenses/lgpl.html>.
 */

using System.Net.Http;
using libredte.api_client.utils;

namespace libredte.api_client
{
    /// <summary>
    /// Clase para interactuar con los endpoints de moneda de la API.
    /// 
    /// Proporciona métodos para realizar operaciones relacionadas con monedas,
    /// como obtener tasas de cambio de moneda para fechas específicas.
    /// </summary>
    public class Sistema : ApiBase
    {
        public Sistema()
        {

        }

        /// <summary>
        /// Obtiene la tasa de cambio de moneda de USD a otra moneda en una fecha específica.
        /// 
        /// Este método realiza una solicitud GET para obtener la tasa de cambio desde USD
        /// hacia la moneda especificada por el usuario para una fecha dada. Es útil para
        /// consultas de conversiones de moneda históricas o actuales.
        /// </summary>
        /// <param name="moneda">String Código de la moneda destino a la cual se desea convertir USD.</param>
        /// <param name="dia">Strng Fecha de la consulta de la tasa de cambio.</param>
        /// <returns>HttpResponseMessage Respuesta JSON con la tasa de cambio desde USD a la moneda especificada en la fecha indicada.</returns>
        public HttpResponseMessage GetMonedaCambios(string moneda, string dia)
        {
            return this.client.Get($"/sistema/general/moneda_cambios/tasa/USD/{moneda}/{dia}");
        }
    }
}
