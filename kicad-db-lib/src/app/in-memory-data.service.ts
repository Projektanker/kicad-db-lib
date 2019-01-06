import { Injectable } from '@angular/core';
import { Hero } from './hero';
import { InMemoryDbService } from 'angular-in-memory-web-api';
import { Part } from './part/part';

@Injectable({
  providedIn: 'root'
})
export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    const heroes: Hero[] = [
      { id: 12, name: 'Narco' },
      { id: 13, name: 'Bombasto' },
      { id: 14, name: 'Celeritas' },
      { id: 15, name: 'Magneta' },
      { id: 16, name: 'RubberMan' },
      { id: 17, name: 'Dynama' },
      { id: 18, name: 'Dr IQ' },
      { id: 19, name: 'Magma' },
      { id: 20, name: 'Tornado' }
    ];
    const parts: Part[] = [
      {
        id: 11,
        reference: 'R',
        value: '1K',
        footprint: 'Resistor_SMD:R_0603_1608Metric',
        library: 'R_0603',
        description: 'Resistor 1K 0603 75V',
        keywords: 'Res Resistor 1K 0603',
        symbol: '{lib}:R',
        datasheet: 'no datasheet'
      },
      {
        id: 12,
        reference: 'R',
        value: '10K',
        footprint: 'Resistor_SMD:R_0603_1608Metric',
        library: 'R_0603',
        description: 'Resistor 10K 0603 75V',
        keywords: 'Res Resistor 10K 0603',
        symbol: '{lib}:R',
        datasheet: 'no datasheet'
      },
      {
        id: 13,
        reference: 'R',
        value: '1K',
        footprint: 'Resistor_SMD:R_0805_2012Metric',
        library: 'R_0805',
        description: 'Resistor 1K 0603 150V',
        keywords: 'Res Resistor 1K 0603',
        symbol: '{lib}:R',
        datasheet: 'no datasheet'
      },
      {
        id: 14,
        reference: 'R',
        value: '10K',
        footprint: 'Resistor_SMD:R_0805_2012Metric',
        library: 'R_0805',
        description: 'Resistor 10K 0805 150V',
        keywords: 'Res Resistor 10K 0805',
        symbol: '{lib}:R',
        datasheet: 'no datasheet'
      },
      {
        id: 15,
        reference: 'C',
        value: '100n_50V',
        footprint: 'Capacitor_SMD:C_0603_1608Metric',
        library: 'C_0603',
        description: 'Capacitor 100nF 50V 0603',
        keywords: 'Cap Capacitor 100n 0603',
        symbol: '{lib}:C',
        datasheet: 'no datasheet'
      },
      {
        id: 16,
        reference: 'C',
        value: '10u_16V',
        footprint: 'Capacitor_SMD:C_0805_2012Metric',
        library: 'C_0805',
        description: 'Capacitor 10uF 0805 16V',
        keywords: 'Capacitor Capacitor 10u 0805',
        symbol: '{lib}:C',
        datasheet: 'no datasheet'
      }
    ];
    return { heroes, parts };
  }

  // Overrides the genId method to ensure that a hero always has an id.
  // If the heroes array is empty,
  // the method below returns the initial number (11).
  // if the heroes array is not empty, the method below returns the highest
  // hero id + 1.
  genId(data: any[]): number {
    return data.length > 0 ? Math.max(...data.map(x => x.id)) + 1 : 11;
  }
}
