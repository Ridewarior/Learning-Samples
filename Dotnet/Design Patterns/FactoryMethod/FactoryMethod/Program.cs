using FactoryMethod.Levels;

Level level1 = LevelFactory.CreateLevel(1);
level1.EncounterEnemy();

Level level2 = LevelFactory.CreateLevel(2);
level2.EncounterEnemy();