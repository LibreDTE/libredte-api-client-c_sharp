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

using System;
using System.Collections.Generic;

namespace libredte.api_client.utils
{
    public class ApiException : Exception
    {
        public int Code;
        public Dictionary<int, string> parameters = new Dictionary<int, string>();

        public ApiException(string message, int code = 0, Dictionary<int, string> parameters = null) : base(message)
        {
            this.Code = code;
            this.parameters = parameters;
        }

        public override string ToString()
        {
            if (this.Code != 0)
            {
                return $"[LibreDTE] Error {this.Code}: {this.Message}";
            }
            else
            {
                return $"[LibreDTE] {this.Message}";
            }
        }
    }
}
