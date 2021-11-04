Logg inn detaljer:
	Brukernavn: admin1234
	Passord: admin1234

Løsningen leveres med database som inneholder admin brukern slik at det er mulig å logge seg inn.
Samtidig det finnes et endpoint som gjør det mulig å registrere et admin bruker dersom det skulle være nødvendig.
(Dersom databasen initialiseres, opprettes det ikke brukeren)
For å registrere et ny admin bruker, brukes følgende endpoint "localhost:x/API/RegisterUser?u=X&p=Y" hvor X = brukernavn og Y = passord.
OBS. ved registrering av et ny admin bruker, må man passe på å ha data av riktig format ellers vil det umulig å logge seg inn (pga. input validering ved logg inn)
"safe format" er 8 alfanumeriske tegn som "admin1234".

Databasen ved behov, initialiseres ved å  "ut-kommentere" linjen "/DBinit.InitializeApplicationDB(app);" på Startup.cs

Det legges til backend fra oppgaven 1 som tillegg for å gi kontekst på hele applikasjonen.
Klient fra oppgave 1 leveres ikke, blant annet for å unngå konflikt mellom klientene. 

