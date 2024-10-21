package model.token;

import com.google.gson.annotations.Expose;
import lombok.Data;

@Data
public class TokenModel {
    @Expose
    private String token;
}
