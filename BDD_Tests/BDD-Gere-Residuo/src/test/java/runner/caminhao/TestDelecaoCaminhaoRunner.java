package runner.caminhao;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "steps.caminhao", "hook.caminhao"},
        tags = "@CAMINHAO and @FUNCIONAL and @DELETE",
        plugin = {"html:target/reports/caminhao/delecao-caminhao-feature/cucumber-reports.html"}
)
public class TestDelecaoCaminhaoRunner {

}
