import com.vaadin.flow.component.html.Div;
import com.vaadin.flow.router.Route;

@JavaScript("frontend://script.js")
@Route
public class MainView extends Div {
    public MainView() {
        search = getElement().executeJavaScript("greet($0, $1)", "client", getElement());
        File file = new File("append.txt");
        FileWriter fr = new FileWriter(file, true);
        fr.write("data");
        fr.close();
    }
}