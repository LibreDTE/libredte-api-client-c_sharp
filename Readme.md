# LibreDTE: Cliente de API en C#

Enlaces sujetos a cambios.

[![NuGet version](https://img.shields.io/nuget/v/libredte.svg)](https://www.nuget.org/packages/libredte/)
[![NuGet downloads](https://img.shields.io/nuget/dt/libredte.svg)](https://www.nuget.org/packages/libredte/)

Cliente para realizar la integración con los servicios web de [LibreDTE](https://www.libredte.cl) desde Python.

## Instalación y actualización

### Instalación mediante el Administrador de Paquetes NuGet

1. Abre tu proyecto en Visual Studio.
2. Haz clic derecho en el proyecto en el Explorador de Soluciones y selecciona "Administrar paquetes NuGet...".
3. En la pestaña "Examinar", busca `libredte`. Debe ser la versión C# (Csharp).
4. Selecciona el paquete `libredte` y haz clic en "Instalar".

### Instalación desde la línea de comandos (cmd)

1. Abre la línea de comandos desde Herramientas, Administrador de paquetes NuGet, Consola del administrador de paquetes.
2. Ejecuta el siguiente comando para instalar `libredte`:

```sh
nuget install libredte
```

### Autenticación en LibreDTE

Lo más simple, y recomendado, es usar una variable de entorno con el [hash del usuario](https://libredte.cl/usuarios/perfil#datos:hashField), la cual será reconocida automáticamente por el cliente:

```sh

    set LIBREDTE_HASH="aquí-tu-hash-de-usuario"

```

Si no se desea usar una variable de entorno, al instanciar los objetos se deberá indicar el hash del usuario. Ejemplo:

```C#

    using libredte;

    string LIBREDTE_HASH="aquí-tu-hash-de-usuario";
    var client = api_client.utils.ApiClient(LIBREDTE_HASH);
```

Si utilizas LibreDTE Edición Comunidad deberás además configurar la URL
de tu servidor. Ejemplo:

```sh

    export LIBREDTE_URL="https://libredte.example.com"
```

Y si deseas hacerlo sin la variable de entorno, debes pasar la URL como
segundo parámetro en el constructor del cliente:

```C#

    using libredte;

    string LIBREDTE_HASH="aquí-tu-hash-de-usuario";
    string LIBREDTE_URL="https://libredte.example.com";
    var client = api_client.utils.ApiClient(LIBREDTE_HASH, LIBREDTE_URL);

```

### Licencia
--------

Este programa es software libre: usted puede redistribuirlo y/o modificarlo
bajo los términos de la GNU Lesser General Public License (LGPL) publicada
por la Fundación para el Software Libre, ya sea la versión 3 de la Licencia,
o (a su elección) cualquier versión posterior de la misma.

Este programa se distribuye con la esperanza de que sea útil, pero SIN
GARANTÍA ALGUNA; ni siquiera la garantía implícita MERCANTIL o de APTITUD
PARA UN PROPÓSITO DETERMINADO. Consulte los detalles de la GNU Lesser General
Public License (LGPL) para obtener una información más detallada.

Debería haber recibido una copia de la GNU Lesser General Public License
(LGPL) junto a este programa. En caso contrario, consulte
[GNU Lesser General Public License](http://www.gnu.org/licenses/lgpl.html).

Enlaces
-------

- [Sitio web LibreDTE](https://www.libredte.cl).
- [Código fuente en GitHub](https://github.com/libredte/libredte-api-client-csharp).
- [Paquete en NuGet](https://www.nuget.org/packages/libredte).
- [Documentación en Read the Docs](https://libredte.readthedocs.io/es/latest).