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

using System.Collections.Generic;
using libredte.api_client.utils;
using System.Net.Http;

namespace libredte.api_client
{
    /// <summary>
    /// Clase para interactuar con los endpoints de cobros de la API.
    /// 
    /// Proporciona métodos para realizar operaciones relacionadas con cobros,
    /// como obtener información sobre cobros específicos y realizar pagos de cobros.
    /// </summary>
    public class Cobros : ApiBase
    {
        public Cobros()
        {

        }

        /// <summary>
        /// Obtiene la información de un cobro específico.
        /// 
        /// Realiza una solicitud POST para buscar información sobre un cobro dado
        /// el identificador del emisor y los datos del cobro.
        /// </summary>
        /// <param name="emisor">String Identificador del emisor del cobro.</param>
        /// <param name="cobro">Dictionary<string, object> Datos del cobro a buscar.</param>
        /// <returns>HttpResponseMessage Respuesta JSON con la información del cobro.</returns>
        public HttpResponseMessage GetCobros(string emisor, Dictionary<string, object> cobro)
        {
            return this.client.Post($"/pagos/cobros/buscar/{emisor}", data: cobro);
        }

        /// <summary>
        /// Realiza el pago de un cobro.
        /// 
        /// Envía una solicitud POST para efectuar el pago de un cobro específico,
        /// identificado por su código y el emisor, utilizando los datos proporcionados
        /// para el pago.
        /// </summary>
        /// <param name="codigo">String Código que identifica el cobro a pagar.</param>
        /// <param name="emisor">String Identificador del emisor del cobro.</param>
        /// <param name="pagarCobro">Dictionary<string, object> Datos del pago a realizar.</param>
        /// <returns>HttpResponseMessage Respuesta JSON del resultado del pago del cobro.</returns>
        public HttpResponseMessage PagarCobro(string codigo, string emisor, Dictionary<string, object> pagarCobro)
        {
            return this.client.Post($"/pagos/cobros/pagar/{codigo}/{emisor}", data: pagarCobro);
        }

        /// <summary>
        /// Obtiene la información de cobro asociada a un DTE temporal específico.
        /// 
        /// Realiza una solicitud POST para buscar información de cobro basada en un DTE temporal,
        /// dado el RUT del receptor, el tipo de DTE, el código del DTE temporal, y el RUT del emisor.
        /// </summary>
        /// <param name="receptor">String RUT del receptor asociado al DTE temporal.</param>
        /// <param name="dte">String Tipo de DTE temporal.</param>
        /// <param name="codigo">String Código único del DTE temporal.</param>
        /// <param name="emisor">String RUT del emisor del DTE temporal.</param>
        /// <returns>HttpResponseMessage Respuesta JSON con la información de cobro del DTE temporal.</returns>
        public HttpResponseMessage GetCobroDteTemporal(string receptor, string dte, string codigo, string emisor)
        {
            return this.client.Get($"/dte/dte_tmps/cobro/{receptor}/{dte}/{codigo}/{emisor}");
        }

        /// <summary>
        /// Obtiene la información de cobro asociada a un DTE real específico.
        /// 
        /// Envia una solicitud POST para buscar información de cobro basada en un DTE real,
        /// dado el tipo de DTE, el folio del DTE, y el RUT del emisor.
        /// </summary>
        /// <param name="dte">String Tipo de DTE real.</param>
        /// <param name="folio">String Folio del DTE real.</param>
        /// <param name="emisor">String RUT del emisor del DTE real.</param>
        /// <returns>HttpResponseMessage Respuesta JSON con la información de cobro del DTE real.</returns>
        public HttpResponseMessage GetCobroDteReal(string dte, string folio, string emisor)
        {
            return this.client.Get($"/dte/dte_emitidos/cobro/{dte}/{folio}/{emisor}");
        }

        /// <summary>
        /// Obtiene información detallada de un cobro específico.
        /// 
        /// Realiza una solicitud POST para recuperar detalles completos sobre un cobro,
        /// identificado por su código y el RUT del emisor. Esta información puede incluir
        /// detalles del estado del cobro, montos, fechas relevantes, entre otros datos
        /// específicos del cobro consultado.
        /// </summary>
        /// <param name="codigo">String Código único que identifica el cobro.</param>
        /// <param name="emisor">String RUT del emisor asociado al cobro.</param>
        /// <returns>HttpResponseMessage Respuesta JSON con la información detallada del cobro.</returns>
        public HttpResponseMessage GetCobroInfo(string codigo, string emisor)
        {
            return this.client.Get($"/pagos/cobros/info/{codigo}/{emisor}");
        }
    }
}
