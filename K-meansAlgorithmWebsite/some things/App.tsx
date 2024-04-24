import { useState } from 'react';
import './App.css';
// import {ScatterPlot} from './ScatterPlot'; // Предполагается, что файл называется ScatterPlot.tsx
// import {DataTable} from './DataTable'; // Предполагается, что файл называется DataTable.tsx

interface DataRow {
  x: number;
  y: number;
  cluster: string;
}

function App() {
    // const [forecasts, setForecasts] = useState<Forecast[]>();

    // useEffect(() => {
    //     populateWeatherData();
    // }, []);

    // async function populateWeatherData() {
    //     const response = await fetch('weatherforecast');
    //     const data = await response.json();
    //     setForecasts(data);
    // }
    // Состояние для данных
    const [data, setData] = useState([]);
    const [clustersCount, setClusters] = useState([]);
      // return (
      //   <div>
      //     <ScatterPlot setSlustersCount={setClusters} setPLotData={setData}/>
      //     <DataTable setMyData = {setData} setCountOfClusters = {setClusters}/>
      //   </div>
      // );
      return;
}
export default App;