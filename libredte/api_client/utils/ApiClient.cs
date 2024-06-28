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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace libredte.api_client.utils
{
    /// <summary>
    /// Cliente API para integrarse con LibreDTE.
    /// 
    /// Esta clase proporciona funcionalidades para interactuar con los servicios web de LibreDTE,
    /// permitiendo realizar operaciones GET y POST, y crear enlaces a recursos de LibreDTE.
    /// 
    /// :param str url: URL base del servicio de LibreDTE.
    /// :param requests.auth.HTTPBasicAuth http_auth: Autenticación para las solicitudes HTTP.
    /// :param bool ssl_check: Indica si se debe verificar el certificado SSL del host.
    /// :param int rut: RUT del contribuyente en LibreDTE.
    /// </summary>
    public class ApiClient
    {
        private const string DEFAULT_URL = "https://libredte.cl";
        private const string DEFAULT_VERSION = "v1";

        public const int AMBIENTE_SII_PRODUCCION = 0;
        public const int AMBIENTE_SII_PRUEBAS = 1;

        private string hash;
        private string url;
        private string version;
        private bool raiseForStatus;
        private string rut;
        private bool sslCheck;
        private string ambienteSii;
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private HttpClient httpClient;

        /// <summary>
        /// Inicializa el cliente API con la configuración necesaria para realizar las solicitudes.
        /// </summary>
        /// <param name="hash" type="String">Hash de autenticación del usuario. Si es None, se intentará obtener de la variable de entorno LIBREDTE_HASH.</param>
        /// <param name="url" type="String">URL base del servicio de LibreDTE. Si es None, se intentará obtener de la variable de entorno LIBREDTE_URL o se usará la URL por defecto.</param>
        /// <param name="version" type="String">Versión de la API a utilizar. Por defecto, se usa una versión predefinida.</param>
        /// <param name="raiseForStatus" type="bool">Si se debe lanzar una excepción automáticamente para respuestas de error HTTP. Por defecto es True.</param>
        /// <exception cref="ApiException">Si el hash del usuario no es válido o está ausente.</exception>
        public ApiClient(string hash = null, string url = null, string version = null, bool raiseForStatus = true)
        {
            this.httpClient = new HttpClient();
            this.SetHash(hash);
            this.url = this.ValidateUrl(url);
            this.headers = this.GenerateHeaders();
            this.version = version ?? DEFAULT_VERSION;
            this.raiseForStatus = raiseForStatus;
            this.SetSsl();
            this.setContribuyente();
            this.SetAmbienteSii();
        }

        /// <summary>
        /// Valida y retorna el hash de autenticación.
        /// </summary>
        /// <param name="hash" type="String">Hash de autenticación a validar.</param>
        /// <returns type="String">Hash validado.</returns>
        /// <exception cref="ApiException">Si el hash no es válido o está ausente.</exception>
        private string ValidateHash(string hash)
        {
            hash = hash ?? Environment.GetEnvironmentVariable("LIBREDTE_HASH");
            if (string.IsNullOrEmpty(hash))
            {
                throw new ApiException("Se debe configurar la variable de entorno: LIBREDTE_HASH.");
            }
            hash = hash.ToString().Trim();
            if (hash.Length != 32)
            {
                throw new ApiException("El hash del usuario debe ser de 32 caracteres.");
            }
            this.hash = hash;
            return this.hash;
        }

        /// <summary>
        /// Valida y retorna la URL base para la API.
        /// </summary>
        /// <param name="url" type="String">URL a validar.</param>
        /// <returns type="String">URL validada.</returns>
        /// <exception cref="ApiException">Si la URL no es válida o está ausente.</exception>
        private string ValidateUrl(string url)
        {
            url = url ?? Environment.GetEnvironmentVariable("LIBREDTE_URL") ?? DEFAULT_URL;

            return url.Trim();
        }

        /// <summary>
        /// Genera y retorna las cabeceras por omisión para las solicitudes.
        /// </summary>
        /// <returns type="Dictionary<string, string>">Dictionary Cabeceras por omisión.</returns>
        private Dictionary<string, string> GenerateHeaders()
        {
            return new Dictionary<string, string>
            {
                {"User-Agent", "LibreDTE-Cliente-de-API-en-C#."},
                {"ContentType", "application/json"},
                {"Accept", "application/json"}
            };
        }

        /// <summary>
        /// Configura las opciones de SSL para las conexiones HTTP.
        /// 
        /// Este método permite activar o desactivar la verificación del certificado SSL
        /// del servidor. Si se activa, las conexiones HTTP verificarán el certificado SSL
        /// del servidor; si se desactiva, no lo harán.
        /// </summary>
        /// <param name="sslCheck" type="bool">Indica si se debe verificar el certificado SSL del host. 
        /// Por omisión es True.</param>
        /// <returns type="bool">El estado actualizado de la verificación del SSL.</returns>
        /// <example>client.SetSsl(false) # Desactiva la verificación SSL</example>
        public bool SetSsl(bool sslCheck = true)
        {
            this.sslCheck = sslCheck;
            return this.sslCheck;
        }

        /// <summary>
        /// Establece el RUT del contribuyente para las solicitudes de la API.
        /// 
        /// Este método permite configurar el RUT del contribuyente que se utilizará en las
        /// solicitudes subsiguientes. Si se proporciona un RUT, este se utiliza; de lo contrario,
        /// se intenta obtener de la variable de entorno 'LIBREDTE_RUT'.
        /// </summary>
        /// <param name="rut" type="String">RUT del contribuyente a establecer. Si es None, se intentará obtener 
        /// de la variable de entorno 'LIBREDTE_RUT'.</param>
        /// <returns type="String">El RUT del contribuyente configurado.</returns>
        /// <exception cref="ApiException">Si el RUT es inválido o no se puede convertir a entero.</exception>
        public string setContribuyente(string rut = null)
        {
            string rutString = rut ?? Environment.GetEnvironmentVariable("LIBREDTE_RUT");
            if (string.IsNullOrEmpty(rutString) == false)
            {
                string tempRutString = rutString;
                if (rutString.Contains('.'))
                {
                    tempRutString = $"{rutString.Split('.')[0]}{rutString.Split('.')[1]}{rutString.Split('.')[2]}";
                }
                if (rutString.Contains('-'))
                {
                    rutString = tempRutString.Split('-')[0];
                }
                try
                {
                    int rutInt = Convert.ToInt32(rutString);
                }
                catch (InvalidCastException)
                {
                    throw new ApiException($"Valor de RUT inválido: {rut}");
                }
            }
            this.rut = rutString;
            return this.rut;
        }

        /// <summary>
        /// Configura la autenticación HTTP básica usando un hash proporcionado.
        /// 
        /// Este método toma un hash, lo valida (o transforma) en un nombre de usuario mediante 
        /// `Validate_hash` y establece la autenticación HTTP básica para su uso en futuras
        /// solicitudes HTTP. La contraseña se establece como un valor predeterminado 'X'.
        /// </summary>
        /// <param name="hash" type="String">Un hash que se valida y se usa para configurar el nombre 
        /// de usuario en la autenticación HTTP básica.</param>
        public void SetHash(string hash)
        {
            string username = this.ValidateHash(hash);
            string password = "X";
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        }

        /// <summary>
        /// Establece el ambiente del Servicio de Impuestos Internos (SII) para las solicitudes de la API.
        /// 
        /// Este método permite configurar el ambiente (producción o pruebas) que se utilizará en las
        /// solicitudes subsiguientes. El ambiente se puede especificar directamente o ser obtenido
        /// de la variable de entorno 'LIBREDTE_AMBIENTE'. Los valores válidos son identificadores
        /// para producción ('0', 'produccion', 'prod', 'palena') y pruebas ('1', 'pruebas', 'test', 'maullin').
        /// </summary>
        /// <param name="ambiente" type="String">Identificador del ambiente a establecer. Si es None, se intentará obtener 
        /// de la variable de entorno 'LIBREDTE_AMBIENTE'.</param>
        /// <returns type="String">El ambiente del SII configurado.</returns>
        /// <exception cref="ApiException">Si el valor proporcionado para el ambiente es inválido.</exception>
        public string SetAmbienteSii(string ambiente = null)
        {
            ambiente = ambiente ?? Environment.GetEnvironmentVariable("LIBREDTE_AMBIENTE");
            if (string.IsNullOrEmpty(ambiente))
            {
                ambiente = null;
            }
            if (!string.IsNullOrEmpty(ambiente))
            {
                ambiente = ambiente.Trim();
                if (ambiente == "0" || ambiente.Equals("produccion", StringComparison.OrdinalIgnoreCase) || ambiente.Equals("prod", StringComparison.OrdinalIgnoreCase) || ambiente.Equals("palena", StringComparison.OrdinalIgnoreCase))
                {
                    ambiente = AMBIENTE_SII_PRODUCCION.ToString();
                }
                else if (ambiente == "1" || ambiente.Equals("pruebas", StringComparison.OrdinalIgnoreCase) || ambiente.Equals("test", StringComparison.OrdinalIgnoreCase) || ambiente.Equals("maullin", StringComparison.OrdinalIgnoreCase))
                {
                    ambiente = AMBIENTE_SII_PRUEBAS.ToString();
                }
                else
                {
                    throw new ApiException($"Valor de Ambiente SII inválido: {ambiente}");
                }
            }
            this.ambienteSii = ambiente;
            return this.ambienteSii;
        }

        /// <summary>
        /// Realiza una solicitud GET a un recurso de la API de LibreDTE.
        /// </summary>
        /// <param name="resource" type="string">Recurso de la API que se desea consumir.</param>
        /// <param name="headers" type="Dictionary<string, string>">Cabeceras adicionales para la solicitud. 
        /// Si es None, se usarán las cabeceras por defecto.</param>
        /// <returns type="HttpResponseMessage">Objeto de respuesta de la solicitud HTTP.</returns>
        /// <exception cref="ApiException">Lanza una excepción si ocurre un error en la solicitud HTTP, como errores de conexión, timeout o HTTP.</exception>
        public HttpResponseMessage Get(string resource, Dictionary<string, string> headers = null)
        {
            return this.SendRequest(method: HttpMethod.Get, resource: resource, headers: headers);
        }

        /// <summary>
        /// Realiza una solicitud POST a un recurso de la API de LibreDTE.
        /// </summary>
        /// <param name="resource" type="string">Recurso de la API que se desea consumir.</param>
        /// <param name="data" type="Dictionary<string, object>">Datos que se enviarán con la solicitud. Si es un diccionario, se codificará como JSON.</param>
        /// <param name="headers" type="Dictionary<string, string>">Cabeceras adicionales para la solicitud. 
        /// Si es None, se usarán las cabeceras por defecto.</param>
        /// <returns type="HttpResponseMessage">Objeto de respuesta de la solicitud HTTP.</returns>
        /// <exception cref="ApiException">Lanza una excepción si ocurre un error en la solicitud HTTP, como errores de conexión, timeout o HTTP.</exception>
        public HttpResponseMessage Post(string resource, Dictionary<string, object> data = null, Dictionary<string, string> headers = null)
        {
            return this.SendRequest(method: HttpMethod.Post, resource: resource, data: data, headers: headers);
        }

        /// <summary>
        /// Método privado para realizar solicitudes HTTP.
        /// </summary>
        /// <param name="method" type="HttpMethod">HttpMethod Método HTTP a utilizar.</param>
        /// <param name="resource" type="string">Recurso de la API a solicitar.</param>
        /// <param name="data" type="Dictionary<string, object>">Dictionary Datos a enviar en la solicitud (opcional).</param>
        /// <param name="headers" type="Dictionary<string, string>">Dictionary Cabeceras adicionales para la solicitud (opcional).</param>
        /// <returns type="HttpResponseMessage">Respuesta de la solicitud.</returns>
        /// <exception cref="ApiException">Si el método HTTP no es soportado o si hay un error de conexión.</exception>
        private HttpResponseMessage SendRequest(HttpMethod method, string resource, Dictionary<string, object> data = null, Dictionary<string, string> headers = null)
        {
            string apiPath = $"/api{resource}";// ruta de la API
            Uri fullUrl = new Uri($"{this.url}{apiPath}"); // URL completa transformada en URI
            HttpRequestMessage request = new HttpRequestMessage(method, fullUrl); // mensaje request compuesto con el método HTTP y la URL
            Dictionary<string, string> extraParams = new Dictionary<string, string>() // Parámetros adicionales
            {
                {"_version", this.version } // Versión
            };
            if (!string.IsNullOrEmpty(this.rut))
            {
                extraParams.Add("_contribuyente_rut", this.rut); // RUT
            }
            if (!string.IsNullOrEmpty(this.ambienteSii))
            {
                extraParams.Add("_contribuyente_certificacion", this.ambienteSii); // Certificación
            }
            fullUrl = new Uri(this.AddParametersToUrl(fullUrl, extraParams));

            System.Diagnostics.Trace.WriteLine(fullUrl.ToString()); // print del URL completo
            if (data != null)
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(data); // Serialización de datos en json
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json"); // Nuevo string content para request, conteniendo jsonData
                }
                catch (JsonSerializationException e)
                {
                    throw new ApiException($"Error al codificar los datos en JSON: {e}"); // Error si no se serializa bien.
                }
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value); // Añadir headers del parámetro
                }
            }
            foreach (var header in this.headers)
            {
                request.Headers.Add(header.Key, header.Value); // Añadir headers generados
            }
            
            try
            {
                var response = this.httpClient.SendAsync(request).Result;
                System.Diagnostics.Trace.WriteLine(response.ToString()); // print de la respuesta completa
                HttpResponseMessage resp = this.CheckAndReturnResponse(response);
                System.Diagnostics.Trace.WriteLine(resp.ToString());
                return resp;
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Trace.WriteLine($"Error en la solicitud: {e}");
                throw new ApiException($"Error en la solicitud: {e}");
            }
            catch (TimeoutException e)
            {
                System.Diagnostics.Trace.WriteLine($"Error de timeout: {e}");
                throw new ApiException($"Error de timeout: {e}");
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine($"Error: {e}");
                throw new ApiException($"Error: {e}");
            }
        }

        /// <summary>
        /// Añade o actualiza parámetros en la URL dada.
        /// 
        /// Esta función toma una URL y un diccionario de parámetros, y devuelve una nueva URL
        /// con los parámetros añadidos o actualizados. Si un parámetro ya existe en la URL,
        /// su valor se actualizará; si no existe, el parámetro se añadirá.
        /// </summary>
        /// <param name="url" type="String">La URL original a la que se añadirán los parámetros.</param>
        /// <param name="extraParams" type="Dictionary<string, string>">Un diccionario de parámetros y valores para añadir a la URL.</param>
        /// <returns type="String">La URL con los parámetros añadidos o actualizados.</returns>
        private string AddParametersToUrl(Uri url, Dictionary<string, string> extraParams)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var param in extraParams)
            {
                query[param.Key] = param.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        /// <summary>
        /// Verifica la respuesta de la solicitud HTTP y maneja los errores.
        /// </summary>
        /// <param name="response" type="HTTPResponseMessage"> Objeto de respuesta de requests.</param>
        /// <returns type="HTTPResponseMessage"> Respuesta de la solicitud.</returns>
        /// <exception cref="ApiException">Si la respuesta contiene un error HTTP.</exception>
        private HttpResponseMessage CheckAndReturnResponse(HttpResponseMessage response)
        {
            if ((int)response.StatusCode != 200 && raiseForStatus)
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                throw new ApiException($"Error HTTP: {errorMessage}");
            }

            return response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Crea un enlace que apunta a un recurso específico en la plataforma de LibreDTE.
        /// </summary>
        /// <param name="resource" type="String">Recurso al que se desea acceder en la plataforma de LibreDTE.</param>
        /// <param name="rut" type="String">RUT del contribuyente en LibreDTE con el que se quiere usar el recurso. Si es None, se usará el RUT almacenado en la clase.</param>
        /// <returns type="String">URL formada para acceder al recurso especificado.</returns>
        /// <exception cref="ApiException">Si el valor de rut no se puede convertir o es nulo/vacío.</exception>
        public string CreateLink(string resource, string rut = null)
        {
            rut = rut ?? this.rut;
            int rutInt;
            try
            {
                if (string.IsNullOrEmpty(rut))
                {
                    throw new InvalidCastException();
                }
                rutInt = Convert.ToInt32(rut);
            }
            catch (InvalidCastException)
            {
                throw new ApiException($"Valor de RUT inválido: {rut}");
            }
            if (!string.IsNullOrEmpty(resource))
            {
                var resourceBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(resource));
                return $"{this.url}/dte/contribuyentes/seleccionar/{rut}/{resourceBase64}";
            }
            else
            {
                return $"{this.url}/dte/contribuyentes/seleccionar/{rut}";
            }
        }
    }
}