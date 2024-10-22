package model.enums;

public enum TipoResiduo {
    GERAL("GERAL"),
    VIDRO("VIDRO"),
    ORGANICO("ORGANICO"),
    PLASTICO("PLASTICO"),
    ELETRONICO("ELETRONICO");


    private String tipoResiduo;

    TipoResiduo(String status) {
        this.tipoResiduo = status;
    }

    public String getStatus() {
        return tipoResiduo;
    }
}
