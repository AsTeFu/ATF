package net.thumbtack.school.elections.model;

public class ItemBulletin {

    private String nameItem;
    private int ID;

    public int getID() {
        return ID;
    }
    public void setID(int ID) {
        this.ID = ID;
    }

    public ItemBulletin(String nameItem) {
        this.nameItem = nameItem;
    }

    public String getName() {
        return nameItem;
    }

}
