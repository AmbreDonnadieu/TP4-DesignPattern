# TP4-DesignPattern
##### par Ambre Donnadieu dans le cadre du cours 8INF956- Programmation avancée de logiciel : Patrons et modèles

## Pour lancer le logiciel : 
- faire dotnet run via l'invite de commande dans le dossier TP4/FileDump
- faire dotnet run via l'invite de commande dans le dossier TP4/LogAnalysis

Ces deux applications receptionnent des messages et doivent donc être lancées en premier.

- faire dotnet run via l'invite de commande dans le dossier TP4/LogEmitter (les messages de log sont écrit aléatoirement pour cet exercice)

A chaque fois que cette application est lancée, elle envoie un message sur le serveur. Ce Message est généré aléatoirement, il n'y a donc pas besoin de l'écrire. A chaque lancement, un seul message est envoyé, c'est pourquoi il faut la relancer en boucle pour avoir plusieurs résultats (messages envoyés).

Le contenu du dossier TutorielRabbitMQ se réfère aux 5 premières parties du tutoriel suivant : https://www.rabbitmq.com/tutorials/tutorial-one-python.html
