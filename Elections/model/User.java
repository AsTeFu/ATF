package net.thumbtack.school.elections.model;

import net.thumbtack.school.elections.database.GeneratorID;

import java.util.Objects;

public class User {

    private int ID;
    private static final GeneratorID generatorID = new GeneratorID();

    private String firstName;
    private String lastName;
    private String patronymic;

    private String address;
    private String building;
    private int apartment;

    private String login;
    private String password;

    public User(String firstName, String lastName, String patronymic, String address, String building, int apartment, String login, String password) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.patronymic = patronymic;
        this.address = address;
        this.building = building;
        this.apartment = apartment;
        this.login = login;
        this.password = password;

        ID = generatorID.generateNext();
    }

    public int getID() {
        return ID;
    }
    public String getFirstName() {
        return firstName;
    }
    public String getLastName() {
        return lastName;
    }
    public String getPatronymic() {
        return patronymic;
    }
    public String getAddress() {
        return address;
    }
    public String getBuilding() {
        return building;
    }
    public int getApartment() {
        return apartment;
    }
    public String getLogin() {
        return login;
    }
    public String getPassword() {
        return password;
    }

    public String getFullName() {
        String name = String.join(" ", lastName, firstName);
        if (!patronymic.equals(""))
            name = String.join(" ", name, patronymic);
        return name;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        User user = (User) o;
        return apartment == user.apartment &&
                Objects.equals(firstName, user.firstName) &&
                Objects.equals(lastName, user.lastName) &&
                Objects.equals(patronymic, user.patronymic) &&
                Objects.equals(address, user.address) &&
                Objects.equals(building, user.building) &&
                Objects.equals(login, user.login) &&
                Objects.equals(password, user.password);
    }

    @Override
    public int hashCode() {
        return Objects.hash(firstName, lastName, patronymic, address, building, apartment, login, password);
    }
}
