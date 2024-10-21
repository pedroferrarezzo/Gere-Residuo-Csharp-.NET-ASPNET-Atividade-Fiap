package model.error;

import com.google.gson.annotations.Expose;
import lombok.Data;

import java.util.List;
@Data
public class ErrorModel {
    @Expose
    private List<String> errorMessages;
}
