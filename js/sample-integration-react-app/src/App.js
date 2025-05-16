import React, { useEffect, useState } from 'react';
import { callHost } from './hostCall'
import { init, registerHostFunc } from './hostEvent';

function App() {
  const [sliderValue, setSliderValue] = useState(1);
  const [objectName, setObjectName] = useState('');
  const [layerName, setLayerName] = useState('');
  const [displayColor, setDisplayColor] = useState('');

  useEffect(() => {
    // スライダーの値が変更されたときにC#に通知
    callHost("UpdateObjectSize", {
      "size": sliderValue
    })
  }, [sliderValue]);

  const setRhinoProperty = (name, layerName, color) => {
    setObjectName(name);
    setLayerName(layerName);
    setDisplayColor(color)
  }

  // C#から任意のタイミングで実行
  init();
  registerHostFunc("setRhinoProperty", setRhinoProperty)

  const handleSliderChange = (event) => {
    const newValue = event.target.value;
    setSliderValue(newValue);
  };

  return (
    <div>
      <h1>Rhino React UI</h1>
      <div>
        <p>オブジェクト名: {objectName}</p>
        <p>レイヤ名: {layerName}</p>
        <p>表示色: {displayColor}</p>
        <div
          style={{
            width: '50px',
            height: '50px',
            backgroundColor: displayColor,
            border: '1px solid #000',
          }}
        ></div>
      </div>
      <label>
        オブジェクトのサイズ倍率:
        <input
          type="range"
          min="0.1"
          max="5"
          step="0.1"
          value={sliderValue}
          onChange={handleSliderChange}
        />
        {sliderValue}
      </label>
    </div>
  );
}

export default App;

