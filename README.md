Not On The List

Un jeu de videur en vue de dessus, fait avec Unity pour la game jam « Uninvited ».

Tu gères l'entrée d'une boîte de nuit. Vérifie chaque client sur la liste des invités, laisse entrer ceux qui y sont, refoule les autres, et garde l'argent et la réputation du club à flot. Laisse entrer les mauvaises personnes et le patron te vire.

<!-- Ajoute une capture d'écran ou un GIF ici : ![Gameplay](docs/gameplay.gif) -->
Gameplay
Clique sur un client dans la file pour lire sa carte d'identité (prénom, nom, âge, métier).
Ouvre la liste des invités et compare. Si le nom y est, Accepter. Sinon, Refuser.
Accepter un client qui est sur la liste rapporte de l'argent et de la réputation. Accepter quelqu'un qui n'y est pas t'en fait perdre.
Le loyer et les dépenses sont prélevés chaque mois, et ils augmentent avec le temps.
Si ton argent tombe trop bas, tu es viré : l'écran de défaite affiche l'argent gagné au total et l'argent qu'il te reste.

Le concept complet prévoit aussi de virer les clients ivres à l'intérieur du club et de fermer les fenêtres avant que des intrus n'entrent.

Fonctionnalités
Système de liste d'invités et de carte d'identité
Argent, dépenses mensuelles et système de réputation par étoiles
Horloge en jeu (heure, jour, mois, année)
Menu principal avec règles du jeu, réglage du volume et meilleur score
Écran de défaite avec l'argent gagné et l'argent restant
Musique en boucle qui continue d'une scène à l'autre
Technologies
Unity 6
C# et TextMeshPro
Musique composée avec FL Studio
Sprites en pixel art
Lancer le projet
Clone le dépôt.
Ouvre le projet avec Unity Hub (utilise la version d'Unity indiquée dans ProjectSettings/ProjectVersion.txt).
Ouvre la scène Menu et appuie sur Play.

Une version web est disponible sur itch.io : (ajoute le lien ici)

Structure du projet
Assets/           scènes, scripts, sprites, audio
Packages/         manifeste des packages Unity
ProjectSettings/
Auteur

Morgan Marcelin (@morgy95)

Réalisé pour la game jam « Uninvited », 2026.
