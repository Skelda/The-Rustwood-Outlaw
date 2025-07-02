# The Rustwood Outlaw

---

## Obsah

- [Popis hry](#popis-hry)
- [Herní mechaniky](#herní-mechaniky)
- [Třídy a architektura](#třídy-a-architektura)
- [Instalace a spuštění](#instalace-a-spuštění)
- [Ovládání](#ovládání)
- [Nastavení](#nastavení)
- [Zdroje a assety](#zdroje-a-assety)
- [Možnosti rozšíření](#možnosti-rozšíření)

---

## Popis hry

Cílem hry je přežít co nejdéle proti vlnám nepřátel, které se spawnují na určených místech mapy. Hráč může sbírat předměty, které mají šanci vypadnout z nepřátel po smrti. Každá úroveň má specifické parametry, jako je rychlost spawnování nepřátel, délka úrovně a rozložení mapy.

---

## Herní mechaniky

- **Barikády**: Slouží k blokování pohybu hráče a nepřátel.
- **SpawnArea**: Speciální typ barikády, kde se v pravidelných intervalech objevují nepřátelé.
- **Nepřátelé (Enemy)**: Spawnují se náhodně na SpawnArea s určitou pravděpodobností určenou zvolenou obtížností.
- **Předměty (Item)**: Lze je sbírat pro různé bonusy (zdraví, rychlost, vylepšení). Mají 10% šanci na objevení se po zabití nepřítele.
- **Hráč (Player)**: Ovládá hlavní postavu, může střílet, pohybovat se a sbírat itemy na mapě.
- **Úrovně (Level)**: Každá úroveň má unikátní mapu, časový limit a obtížnost.

---

## Třídy a architektura

### Hlavní třídy

- **Board**  
  Hlavní herní plocha, spravuje všechny entity atd. v hlavní herní smyččce.

- **Barricade**  
  Základní třída pro barikády. Obsahuje logiku pro vykreslení, pozici na mřížce a odstranění z herní plochy.

- **SpawnArea**  
  Dědí z Barricade. Navíc obsahuje logiku pro spawnování nepřátel.

- **Entity**  
  Základní třída pro všechny pohyblivé objekty (hráč, nepřátelé). Obsahuje vlastnosti jako zdraví, rychlost, poškození a metody pro pohyb, kolize a zničení.

- **Player**  
  Dědí z Entity. Obsahuje specifické vlastnosti a metody pro ovládání hráče, animace a střelbu.

- **Enemy**
  Dědí z Entity. Obsahuje pathfinding, kterým se snaží přiblížit se k hráči.

- **Item**  
  Reprezentuje předměty na mapě, které může hráč sbírat.

- **Level**  
  Uchovává informace o úrovni: název, časový limit, rychlost spawnování, počet bossů a rozložení mapy.

### Další komponenty

- **GameSettings**  
  Statická třída s globálními nastaveními hry (velikost buňky, rychlost nepřátel, šance na spawn, atd.).

- **Properties/Resources.resx**  
  Obsahuje grafické assety (sprite barikády, spawn area, hráč, nepřátelé, atd.).

---

## Instalace a spuštění

1. Spusťte konzoli na Windows `Win + R`
2. Nakloujte git repozitář `git clone https://github.com/Skelda/The-Rustwood-Outlaw`
3. Otevřete repozitář `cd The-Rustwood-Outlaw`
4. Spusťte hru `"The Rustwood Outlaw.exe"`

---

## Ovládání

- **Pohyb**: WASD
- **Střelba**: Šipky
- **Pauza**: P
- **Výběr obtížnosti**: ComboBox v menu pauzy

---

## Nastavení

Ve třídě `GameSettings` lze upravit:

- Velikost buňky (`CellSize`)
- Rychlost nepřátel (`EnemySpeed`)
- Zdraví nepřátel (`EnemyHealth`)
- Poškození nepřátel (`EnemyDamage`)
- Šance na spawn nepřítele (`enemySpawnChance`)
- Rychlost střelby hráče (`PlayerShootingSpeed`)
- Délka trvání předmětů na zemi (`itemOnGroundTime`)
- Další globální parametry

---

## Možnosti rozšíření

- Přidání nových typů nepřátel a bossů
- Vylepšení AI nepřátel
- Nové typy předmětů a power-upů
- Více úrovní a map
- Ukládání a načítání postupu

---

**Autor:**  
Jakub Endlicher
