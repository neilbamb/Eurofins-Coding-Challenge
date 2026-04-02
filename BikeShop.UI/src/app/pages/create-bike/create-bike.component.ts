import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BikeService, type CreateBikeRequest, type Bike } from '../../bike.service';

@Component({
  selector: 'app-create-bike',
  templateUrl: './create-bike.component.html',
  styleUrls: ['./create-bike.component.scss']
})
export class CreateBikeComponent {
  model: CreateBikeRequest = {
    manufacturer: '',
    model: '',
    category: '',
    price: '',
    colour: '',
    weight: '',
    img_url: ''
  };

  error: string | null = null;

  constructor(private bikeService: BikeService, private router: Router) { }

  submit(): void {
    this.error = null;

    this.bikeService.create(this.model).subscribe({
      next: (created: Bike) => {
        this.router.navigate(['/details', created.ref]);
      },
      error: () => {
        this.error = 'Failed to create bike. Please check your inputs.';
      }
    });
  }
}

