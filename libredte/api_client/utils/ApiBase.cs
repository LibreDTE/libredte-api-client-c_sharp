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

namespace libredte.api_client.utils
{
    public class ApiBase
    {
        public ApiClient client;
        /// <summary>
        /// Clase base para las clases que consumen la API (wrappers).
        /// </summary>
        /// <param name="apiHash" type="string">Hash de autenticación del usuario. Si es None, se intentará obtener de la variable de entorno LIBREDTE_HASH.</param>
        /// <param name="apiUrl" type="string">URL base del servicio de LibreDTE. Si es None, se intentará obtener de la variable de entorno LIBREDTE_URL o se usará la URL por defecto.</param>
        /// <param name="apiVersion" type="string">Versión de la API a utilizar. Por defecto, se usa una versión predefinida.</param>
        /// <param name="apiRaiseForStatus" type="bool">Si se debe lanzar una excepción automáticamente para respuestas de error HTTP. Por defecto es True.</param>
        /// <param name="kwargs" type="Dictionary(string, string)">Argumentos adicionales para la autenticación.</param>
        public ApiBase(string apiHash = null, string apiUrl = null, string apiVersion = null, bool apiRaiseForStatus = true, Dictionary<string, string> kwargs = null)
        {
            this.client = new ApiClient(apiHash, apiUrl, apiVersion, apiRaiseForStatus);
        }
    }
}
