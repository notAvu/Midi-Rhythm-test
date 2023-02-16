# Midi-Rhythm-test
Following tutorials to learn about midi management for rhythm games


TODO: 
- Plantear el parseo del midi a un formato que instancie las notas en la escena a cierta distancia dada por los beats y seguir desarrollando entorno a eso (UPDATE: he empezado a trabajar en una prueba con una aplicacion y JSONs para crear los beatmaps)<br>
- Establecer una regla para las distancias entre objetos basado en los bpm (UPDATE: Se hace instanciando cada nota en su timestamp correspondiente, pero creo que hay leves desfases)<br>
- Cambiar el sistema de Lanes para que cada columna lleve su lane en el propio objeto, permitiendo cambiar las notas de Lane durante la cancion 

RECURSOS: <br>
Osu! github repo: https://github.com/ppy/osu

A Dance of fire and ice's creator guide on rhythm: https://web.archive.org/web/20200716132250/http://ludumdare.com/compo/2014/09/09/an-ld48-rhythm-game-part-2/

NoteEditor by setchi (The tool I used to generate beatmaps and export them to a JSON file): https://github.com/setchi/NoteEditor
