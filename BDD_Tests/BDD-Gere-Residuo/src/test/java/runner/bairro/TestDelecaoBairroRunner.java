package runner.bairro;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "steps.bairro", "hook.bairro"},
        tags = "@BAIRRO and @FUNCIONAL and @DELETE",
        plugin = {"html:target/reports/bairro/delecao-bairro-feature/cucumber-reports.html"}
)
public class TestDelecaoBairroRunner {

}
