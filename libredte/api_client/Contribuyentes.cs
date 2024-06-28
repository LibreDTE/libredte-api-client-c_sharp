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
    /// Clase para interactuar con los endpoints de contribuyentes de la API.
    /// 
    /// Ofrece métodos para obtener información de contribuyentes específicos,
    /// basándose en su RUT.
    /// </summary>
    public class Contribuyentes : ApiBase
    {
        public Contribuyentes()
        {

        }

        /// <summary>
        /// Obtiene la información de un contribuyente específico.
        /// 
        /// Realiza una solicitud GET para buscar información sobre un contribuyente
        /// dado su RUT. Esta información puede incluir datos como el nombre del
        /// contribuyente, dirección, y otros detalles relevantes.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public HttpResponseMessage GetContribuyente(string rut)
        {
            return this.client.Get($"/dte/contribuyentes/info/{rut}");
        }
    }
}
