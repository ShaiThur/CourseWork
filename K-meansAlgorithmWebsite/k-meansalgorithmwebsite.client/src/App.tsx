import React, { useState } from 'react';
import Graph3D from './ThreeGraph';

interface DataPoint {
  x: number;
  y: number;
  z: number;
}

interface DataRequest {
 rawData: number[][];
 clustersCount: number;
}

interface ClusterResponse {
 clusteringResult: number[] | null;
}

interface RegressionResponse {
  Coefficients: number[] | null;
}

const App: React.FC = () => {
  let [x, setX] = useState<number>(0);
  let [y, setY] = useState<number>(0);
  let [z, setZ] = useState<number>(0);
  const [clusters, setClusters] = useState<number>(0);
  const [dataPoints, setDataPoints] = useState<DataPoint[]>([]);
  const [clusterResponse, setClusterResponse] = useState<ClusterResponse | null>(null);
  const [coeffResponse, setCoeffResponse] = useState<RegressionResponse | null>(null);

  const dataRequest: DataRequest = {
    rawData: dataPoints.map(point => [point.x, point.y, point.z]),
    clustersCount: clusters,
   };

  const handleAddClick = () => {
    let newDataPoint = { x, y, z };

    setDataPoints([...dataPoints, newDataPoint]);
    setX(0);
    setY(0);
    setZ(0);
  };

  async function sendData(dataRequest : DataRequest, path : string) {
    const clusterResponse = await fetch(path, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(dataRequest)
    });

    if (!clusterResponse.ok) {
      alert(`HTTP error! status: ${clusterResponse.status}`);
      throw new Error(`HTTP error! status: ${clusterResponse.status}`);
    }

    const dataResponse = await clusterResponse.json();
    if (dataResponse) {
      return dataResponse;
    } else {
      alert(`HTTP error! status: ${clusterResponse.status}`);
      throw new Error('Получен пустой ответ от сервера');
    }
  }


  const addData = async () => {
    const res = await sendData(dataRequest, 'algorithm/ClusterData');
    // const coefs = await sendData(dataRequest, 'algorithm/ReceiveCoefficients');

    setClusterResponse(res);
    // setCoeffResponse(coefs);
  }

  let datasets : number[][] = [];
  const updateGraph = async () => {
    dataPoints.forEach((point : DataPoint, index) => {
      if (!datasets[index]){
        datasets.push(
          [
          point.x,
          point.y,
          point.z
          ]
        );
      }
    });
  }

  return (
    <div>

      <input type="number" placeholder='' title='X' value={x} onChange={(e) => setX(Number(e.target.value))} />
      <input type="number" placeholder='' title='Y' value={y} onChange={(e) => setY(Number(e.target.value))} />
      <input type="number" placeholder='' title='Z' value={z} onChange={(e) => setZ(Number(e.target.value))} />
      <input type="number" placeholder='' title='clusters' value={clusters} onChange={(e) => setClusters(Number(e.target.value))} />
      <button onClick={handleAddClick}>Add</button>
      <button onClick={addData}>Send</button>
      <button onClick={updateGraph}>Update</button>

      <Graph3D dataPoints = { datasets } clusters={ clusterResponse?.clusteringResult } />

      <div>Clusters count: {clusters}</div>
      {clusterResponse && (
      <div>
        <div>clusters: {JSON.stringify(clusterResponse.clusteringResult)}</div>
      </div>
      )}

      {coeffResponse && (
      <div>
        <div>clusters: {JSON.stringify(coeffResponse.Coefficients)}</div>
      </div>
      )}

      <div>
      <table>
        <thead>
          <tr>
            <th>X</th>
            <th>Y</th>
            <th>Z</th>
          </tr>
        </thead>
        <tbody>
          {dataPoints.map((point, index) => (
            <tr key={index}>
              <td>{point.x}</td>
              <td>{point.y}</td>
              <td>{point.z}</td>
            </tr>
          ))}
        </tbody>
      </table>
      </div>
    </div>
  );
}

export default App;