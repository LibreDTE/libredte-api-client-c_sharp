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
using System.Text;
using System.Net.Http;
using libredte.api_client.utils;
using System.Net;

namespace libredte.api_client
{
    /// <summary>
    /// Clase para interactuar con los endpoints de DTE de la API.
    /// 
    /// Esta clase hereda de ApiBase y proporciona métodos específicos para operaciones relacionadas con DTE,
    /// como obtener información del receptor, emitir y generar DTEs, tanto temporales como reales, y enviar DTEs por correo electrónico.
    /// </summary>
    public class Dte : ApiBase
    {
        public Dte()
        {

        }

        /// <summary>
        /// Función para convertir filtros a un string válido para la URL
        /// </summary>
        /// <param name="filtros">Dictionary<string, string> Filtros a convertir.</param>
        /// <returns>String Filtros en un string listos para ser usados en la URL.</returns>
        private string ConvertToUrlEncodedString(Dictionary<string, string> filtros)
        {
            var queryBuilder = new StringBuilder();
            foreach (var filtro in filtros)
            {
                queryBuilder.AppendFormat("{0}={1}&", WebUtility.UrlEncode(filtro.Key), WebUtility.UrlEncode(filtro.Value));
            }

            // Remove the trailing '&' if it exists
            if (queryBuilder.Length > 0)
            {
                queryBuilder.Length--; // Remove the last '&'
            }

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Emite un DTE temporal.
        /// </summary>
        /// <param name="dteTemporal">Dictionary<string, object> Datos del DTE temporal a emitir.</param>
        /// <param name="filtros">Dictionary<string, string> Parámetros adicionales para la consulta (opcional).</param>
        /// <returns>HttpResponseMessage Respuesta JSON del DTE temporal emitido.</returns>
        public HttpResponseMessage EmitirDteTemporal(Dictionary<string, object> dteTemporal, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Post($"/dte/documentos/emitir?{filtros}", data : dteTemporal);
        }

        /// <summary>
        /// Obtiene información de un DTE temporal específico.
        /// </summary>
        /// <param name="receptor">String RUT del receptor.</param>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="codigo">String Código del DTE temporal.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="filtros">Dictionary<string, string> Parámetros adicionales para la consulta (opcional).</param>
        /// <returns>HttpREsponseMessage Respuesta JSON con la información del DTE temporal.</returns>
        public HttpResponseMessage GetDteTemporal(string receptor, string dte, string codigo, string emisor, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Get($"/dte/dte_tmps/info/{receptor}/{dte}/{codigo}/{emisor}?{filtros}");
        }

        /// <summary>
        /// Genera un DTE real a partir de los datos proporcionados, correspondientes aun dte temporal.
        /// </summary>
        /// <param name="dteReal">Dictionary<string, object> Datos del DTE real a generar.</param>
        /// <param name="filtros">Dictionary<string, string> Parámetros adicionales para la consulta (opcional).</param>
        /// <returns>HttpResponseMessage Respuesta JSON del DTE real generado.</returns>
        public HttpResponseMessage EmitirDteReal(Dictionary<string, object> dteReal, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Post($"/dte/documentos/generar?{filtros}", data: dteReal);
        }

        /// <summary>
        /// Obtiene información de un DTE real específico.
        /// </summary>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="folio">String Folio del DTE.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="filtros">Dictionary<string, string> Parámetros adicionales para la consulta (opcional).</param>
        /// <returns>HttpResponseMessage Respuesta JSON con la información del DTE real.</returns>
        public HttpResponseMessage GetDteReal(string dte, string folio, string emisor, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Get($"/dte/dte_emitidos/info/{dte}/{folio}/{emisor}?{filtros}");
        }

        /// <summary>
        /// Envía por correo electrónico un DTE temporal.
        /// </summary>
        /// <param name="receptor">String RUT del receptor.</param>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="codigo">String Código del DTE temporal.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="dataEmail">Dictionary<string, object> Datos del correo electrónico a enviar.</param>
        /// <returns>HttpResponseMessage Respuesta JSON del resultado del envío.</returns>
        public HttpResponseMessage DteTemporalEnviarEmail(string receptor, string dte, string codigo, string emisor, Dictionary<string, object> dataEmail)
        {
            return this.client.Post($"/dte/dte_tmps/enviar_email/{receptor}/{dte}/{codigo}/{emisor}", data : dataEmail);
        }

        /// <summary>
        /// Envía por correo electrónico un DTE real.
        /// </summary>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="folio">String Folio del DTE.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="dataEmail">Dictionary<string, object> Datos del correo electrónico a enviar.</param>
        /// <returns>HttpResponseMessage Respuesta JSON del resultado del envío.</returns>
        public HttpResponseMessage DteRealEnviarEmail(string dte, string folio, string emisor, Dictionary<string, object> dataEmail)
        {
            return this.client.Post($"/dte/dte_emitidos/enviar_email/{dte}/{folio}/{emisor}", data : dataEmail);
        }

        /// <summary>
        /// Obtiene el PDF de un DTE real específico.
        /// </summary>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="folio">String Folio del DTE.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="filtros">Dictionary<string, string> Parámetros adicionales para la consulta (opcional).</param>
        /// <returns>HttpResponseMessage Respuesta con el PDF del DTE real.</returns>
        public HttpResponseMessage GetPdfReal(string dte, string folio, string emisor, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Get($"/dte/dte_emitidos/pdf/{dte}/{folio}/{emisor}?{filtros}");
        }

        /// <summary>
        /// Busca DTEs emitidos por un emisor específico utilizando filtros de búsqueda.
        /// </summary>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="filtros">Dictionary<string, object> Filtros de búsqueda para aplicar.</param>
        /// <returns>HttpResponseMessage Filtros de búsqueda para aplicar.</returns>
        public HttpResponseMessage GetDteEmitidos(string emisor, Dictionary<string, object> filtros)
        {
            return this.client.Post($"/dte/dte_emitidos/buscar/{emisor}", data : filtros);
        }

        /// <summary>
        /// Actualiza el estado de un DTE emitido.
        /// </summary>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="folio">String Folio del DTE.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="filtros">Dictionary<string, string> Parámetros adicionales para la actualización de estado (opcional).</param>
        /// <returns>HttpResponseMessage Respuesta JSON con el resultado de la actualización del estado del DTE.</returns>
        public HttpResponseMessage DteEmitidosActualizarEstado(string dte, string folio, string emisor, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Get($"/dte/dte_emitidos/actualizar_estado/{dte}/{folio}/{emisor}?{filtros}"); // NO SE PUEDE PROBAR POR AHORA
        }

        /// <summary>
        /// Consulta los DTEs emitidos utilizando filtros de búsqueda.
        /// </summary>
        /// <param name="filtros">Dictionary<string, object> Filtros de búsqueda para aplicar en la consulta.</param>
        /// <returns>HttpResponseMessage Respuesta JSON con los DTEs emitidos que coinciden con los filtros.</returns>
        public HttpResponseMessage DteEmitidosConsultar(Dictionary<string, object> filtros)
        {
            return this.client.Post($"/dte/dte_emitidos/consultar", data : filtros); // NO SE PUEDE PROBAR POR AHORA
        }

        /// <summary>
        /// Obtiene el TED (Timbre Electrónico de DTE) de un DTE emitido.
        /// </summary>
        /// <param name="dte">String Tipo de DTE.</param>
        /// <param name="folio">String Folio del DTE.</param>
        /// <param name="emisor">String RUT del emisor.</param>
        /// <param name="filtros">Dictionary<string, object> Parámetros adicionales para la consulta (opcional).</param>
        /// <returns>HttpResponseMessage Respuesta con el TED del DTE solicitado.</returns>
        public HttpResponseMessage DteEmitidosTed(string dte, string folio, string emisor, Dictionary<string, string> filtros = null)
        {
            string queryString = filtros == null ? string.Empty : ConvertToUrlEncodedString(filtros);
            return this.client.Get($"/dte/dte_emitidos/ted/{dte}/{folio}/{emisor}?{filtros}"); // NO SE PUEDE PROBAR POR AHORA
        }
    }
}
