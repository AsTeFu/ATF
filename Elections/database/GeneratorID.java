package net.thumbtack.school.elections.database;

// REVU лучше IdGenerator. Кстати, даже если аббревиатура, то все равно camelCase - UserDao, а не UserDAO
public class GeneratorID {
    private int currentID;

    public GeneratorID() {
        this(1);
    }
    public GeneratorID(int startID) {
        currentID = startID;
    }

    public int generateNext() {
        return currentID++;
    }
}
