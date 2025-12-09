# ?? Docker Deployment Guide - P10 Diabetes Microservices

## ?? Prérequis

- Docker Desktop installé et démarré
- Au moins 8 GB de RAM disponible pour Docker
- Ports disponibles : 1433, 27017, 5000, 7008, 7010, 7050, 7070, 7080

---

## ?? Démarrage rapide

### 1. **Build des images Docker**

```bash
docker-compose build
```

Cette commande va :
- Compiler tous les microservices
- Créer les images Docker optimisées
- Cela peut prendre 5-10 minutes la première fois

### 2. **Démarrer tous les services**

```bash
docker-compose up -d
```

Les services démarreront dans cet ordre :
1. SQL Server (port 1433)
2. MongoDB (port 27017)
3. IdentityService (port 7010)
4. PatientService (port 7070)
5. NoteService (port 7080)
6. Gateway Ocelot (port 7050)
7. AssessmentService (port 7008)
8. Frontend (port 5000)

### 3. **Vérifier que tout fonctionne**

```bash
docker-compose ps
```

Tous les services doivent être à l'état `Up`.

---

## ?? URLs d'accès

| Service | URL | Description |
|---------|-----|-------------|
| **Frontend** | http://localhost:5000 | Application web principale |
| **Gateway** | http://localhost:7050 | API Gateway (Ocelot) |
| **IdentityService** | http://localhost:7010 | Service d'authentification |
| **PatientService** | http://localhost:7070 | Gestion des patients |
| **NoteService** | http://localhost:7080 | Gestion des notes |
| **AssessmentService** | http://localhost:7008 | Évaluation des risques |
| **SQL Server** | localhost:1433 | Base de données (sa/YourStrong!Passw0rd) |
| **MongoDB** | localhost:27017 | Base de données NoSQL |

---

## ?? Commandes utiles

### Voir les logs en temps réel

```bash
# Tous les services
docker-compose logs -f

# Un service spécifique
docker-compose logs -f frontend
docker-compose logs -f gateway
docker-compose logs -f patientservice
```

### Arrêter les services

```bash
docker-compose down
```

### Arrêter et supprimer les volumes (données)

```bash
docker-compose down -v
```

### Redémarrer un service spécifique

```bash
docker-compose restart frontend
```

### Reconstruire un service spécifique

```bash
docker-compose build frontend
docker-compose up -d frontend
```

---

## ?? Dépannage

### Problème : Les services ne démarrent pas

**Solution :** Vérifier les logs
```bash
docker-compose logs
```

### Problème : Erreur de connexion à la base de données

**Solution :** Attendre que SQL Server soit complètement démarré
```bash
docker-compose logs sqlserver
```

Chercher le message : `SQL Server is now ready for client connections`

### Problème : Port déjà utilisé

**Solution :** Modifier les ports dans `docker-compose.yml`
```yaml
ports:
  - "5001:80"  # au lieu de 5000:80
```

### Problème : Manque de mémoire

**Solution :** Augmenter la mémoire allouée à Docker Desktop (Settings > Resources > Memory)

---

## ?? Configuration

### Variables d'environnement

Tous les services utilisent `ASPNETCORE_ENVIRONMENT=Docker` qui charge automatiquement les fichiers `appsettings.Docker.json`.

### Bases de données

**SQL Server :**
- User: `sa`
- Password: `YourStrong!Passw0rd`
- Databases: `AuthDb`, `PatientDb`

**MongoDB :**
- No authentication
- Database: `NotesDb`

---

## ?? Tests

### Tester l'authentification

```bash
curl -X POST http://localhost:7050/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'
```

### Tester la gateway

```bash
curl http://localhost:7050/api/patients \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## ?? Nettoyage complet

Pour supprimer tous les conteneurs, images et volumes :

```bash
docker-compose down -v --rmi all
```

---

## ?? Notes importantes

1. **Première exécution :** Les migrations EF Core s'exécutent automatiquement au démarrage
2. **Health checks :** Les services attendent que les bases de données soient prêtes
3. **Réseau :** Tous les services communiquent via un réseau Docker privé `p10-network`
4. **Persistance :** Les données SQL et MongoDB sont persistées dans des volumes Docker

---

## ?? Architecture Docker

```
???????????????
?   Browser   ?
???????????????
       ?
       v
???????????????
?  Frontend   ? :5000
???????????????
       ?
       v
???????????????
?   Gateway   ? :7050
???????????????
       ?
   ??????????????????????????????????????????????
   v               v              v             v
???????????  ????????????  ????????????  ??????????????
?Identity ?  ? Patient  ?  ?   Note   ?  ?Assessment  ?
?  :7010  ?  ?  :7070   ?  ?  :7080   ?  ?   :7008    ?
???????????  ????????????  ????????????  ??????????????
     ?            ?              ?              ?
     v            v              v              ?
????????????  ????????????                     ?
?   SQL    ?  ? MongoDB  ?                     ?
?  Server  ?  ?  :27017  ?                     ?
?  :1433   ?  ????????????                     ?
????????????                                    ?
     ?                                          ?
     ????????????????????????????????????????????
```

---

**Bon déploiement ! ??**
