package model.enums;

public enum StatusColetaDeLixo {
    EM_ANDAMENTO("EM_ANDAMENTO"),
    FINALIZADA("FINALIZADA");


    private String status;

    StatusColetaDeLixo(String status) {
        this.status = status;
    }

    public String getStatus() {
        return status;
    }
}
