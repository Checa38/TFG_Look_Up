# Look Up 🌌  
Aplicación de Realidad Aumentada para la Divulgación de Astronomía

![Unity](https://img.shields.io/badge/engine-Unity-000?logo=unity&logoColor=white)
![Platform](https://img.shields.io/badge/platform-Android-green)
![License](https://img.shields.io/badge/license-Educational-blue)

## 📱 Descripción

**Look Up** es una aplicación móvil desarrollada con Unity que utiliza **realidad aumentada (AR)** para ayudar a los usuarios a redescubrir el cielo desde cualquier entorno, incluso desde ciudades con alta contaminación lumínica. Esta app combina la observación astronómica con mecánicas de videojuegos para ofrecer una experiencia educativa, interactiva y entretenida.

Inspirada por herramientas como Stellarium y juegos como Pokémon Go, Look Up permite visualizar estrellas, constelaciones y planetas en tiempo real, desbloquear logros y acceder a contenido multimedia informativo. Todo esto enmarcado en una interfaz intuitiva, multilingüe y accesible.

---

## 🧩 Estructura del Proyecto

- `Datos`: Contiene ejemplos y archivos `.json` con progresos de usuario y logros.  
- `Documentos`: Documentación del proyecto, incluyendo manual técnico y memoria del TFG.  
- `Localization`: Archivos de localización en español, inglés y francés mediante Unity Localization.  
- `Packages`: Dependencias y configuración de paquetes del proyecto Unity.  
- `Petición`: Scripts y herramientas para realizar peticiones al servidor/API (incluye uso de Stellarium API).  
- `Prefabs`: Prefabricados reutilizables de Unity (astros, HUD, paneles informativos, etc.).  
- `ProjectSettings`: Configuraciones de proyecto específicas de Unity.  
- `Scenes`: Escenas del juego como login, juego principal, menú, ajustes y tutorial.  
- `Scripts`: Código fuente en C#, organizado por funcionalidades (gestión de usuario, RA, logros, etc.).  
- `UserSettings`: Configuraciones locales del usuario para desarrollo con Unity.

---

## 🔧 Tecnologías Usadas

- **Unity** (AR Foundation)
- **C#** como lenguaje de programación principal
- **XAMPP** (MySQL + Apache)
- **Ngrok** para pruebas en red
- **Adobe Photoshop / Flaticon** para diseño gráfico
- **Stellarium API** para información astronómica
- **GitHub** para control de versiones
- **Figma** para prototipado

---

## 🚀 Funcionalidades Clave

- 🔭 Visualización de astros en RA según ubicación real
- 🧠 Aprendizaje interactivo con contenido multimedia y fichas informativas
- 🏆 Sistema de logros y progreso personalizado
- 🌐 Multilenguaje: Español, Inglés, Francés
- 🔐 Gestión segura de cuentas de usuario (registro/login)
- 🗺️ Posibilidad de simular diferentes ubicaciones geográficas

---

## 🧪 Pruebas

Se han realizado pruebas de caja negra y caja blanca para validar funcionalidades como:
- Registro e inicio de sesión
- Visualización de objetos en RA
- Guardado y carga de progreso
- Respuesta ante errores (credenciales, conexión, etc.)
- Comprobación de seguridad básica (SQL Injection)

---

## 📄 Licencia

Este proyecto ha sido desarrollado como Trabajo de Fin de Grado (TFG) en Ingeniería Informática. Uso educativo y académico únicamente.

Autor: **Carlos Checa Moreno**  
Director: **Cristóbal Romero Morales**  
Universidad de Córdoba · Septiembre 2024

---

## 📬 Contacto

carloschecamoreno@gmail.com

