import Typography from "@mui/material/Typography";
import NavBar from "../components/NavBar";

type Props = {
    children?: React.ReactNode;
};

export const DefaultLayout: React.FC<Props> = ({children}) => {
    return (
        <>
            <NavBar />
            <Typography variant="h5" align="center">Welcome to the Microsoft Authentication Library For React Quickstart</Typography>
            <br/>
            <br/>
            {children}
        </>
    );
};