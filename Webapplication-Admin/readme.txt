Logg inn detaljer:
	Brukernavn: admin1234
	Passord: admin1234

Løsningen leveres med database som inneholder admin brukern slik at det er mulig å logge seg inn.
Samtidig det finnes et endpoint som gjør det mulig å registrere et admin bruker dersom det skulle være nødvendig.
For å registrere et ny admin bruker, brukes følgende endpoint ".../API/RegisterUser?u=X&p=Y" hvor X = brukernavn og Y = passord.
OBS. ved registrering av et ny admin bruker må man passe på å ha data av riktig format ellers vil det umulig å logge seg inn (pga. input validering ved logg inn)
"safe format" er 8 alfanumeriske tegn som "admin1234"