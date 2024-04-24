import { useRef, useState } from "react";



export const DataTable = ({ setMyData, setCountOfClusters }) => {

    let point_x = useRef(null);
    let point_y = useRef(null);
    let cluster_count = useRef(null);
    const [data, setData] = useState([])
    const [clusters, setClusters] = useState([]);

    const AddDataPoints = () => {
        let newPoint = {x: +point_x.current.value, y: +point_y.current.value}
        let newCount = {k: +cluster_count.current.value}
        setData([...data, newPoint])
        setMyData(data)
        clusters.pop();
        setClusters([...clusters, newCount])
        setCountOfClusters(clusters)
    }

    return (
        <>
        <input type="text" placeholder="X" ref={point_x}/>
        <input type="text" placeholder="Y" ref={point_y}/>
        <input type="text" placeholder="clusters" ref={cluster_count}/>
        <button onClick={() => AddDataPoints()}>Add</button>

        <label>clusters count</label>
        {clusters.map((row) => (
              <label> {row.k}</label>
          ))}
        <table>
        <thead>
          <tr>
            <th>X</th>
            <th>Y</th>
          </tr>
        </thead>
        <tbody>
          {data.map((row, index) => (
            <tr key={index}>
              <td>{row.x}</td>
              <td>{row.y}</td>
            </tr>
          ))}
        </tbody>
      </table>
        </>
    );
}