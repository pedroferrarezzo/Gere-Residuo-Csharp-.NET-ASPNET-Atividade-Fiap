package runner.morador;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "steps.bairro", "steps.morador", "hook.morador", "hook.bairro"},
        tags = "@MORADOR and @FUNCIONAL and @DELETE",
        plugin = {"html:target/reports/morador/delecao-morador-feature/cucumber-reports.html"}
)
public class TestDelecaoMoradorRunner {

}
