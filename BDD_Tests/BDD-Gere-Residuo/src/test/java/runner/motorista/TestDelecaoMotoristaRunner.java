package runner.motorista;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "steps.motorista", "hook.motorista"},
        tags = "@MOTORISTA and @FUNCIONAL and @DELETE",
        plugin = {"html:target/reports/motorista/delecao-motorista-feature/cucumber-reports.html"}
)
public class TestDelecaoMotoristaRunner {

}
