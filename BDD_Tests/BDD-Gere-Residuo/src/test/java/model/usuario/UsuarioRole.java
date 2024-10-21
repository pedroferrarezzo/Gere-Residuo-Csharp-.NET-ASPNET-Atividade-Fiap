package model.usuario;

public enum UsuarioRole {
    ADMIN("ADMIN"),
    OPERADOR("OPERADOR"),
    USER("USER");

    private String role;

    UsuarioRole(String role) {
        this.role = role;
    }

    public String getRole() {
        return role;
    }

}
